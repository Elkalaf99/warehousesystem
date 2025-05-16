using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Forms;

/// <summary>
/// Form for generating and viewing reports.
/// </summary>
public partial class ReportsForm : Form
{
    private WarehouseContext _context;
    private DataGridView _gridView;
    private ComboBox _productFilter;
    private DateTimePicker _dateFrom;
    private DateTimePicker _dateTo;
    private Button _generateButton;
    private Button _exportButton;
    private Button _printButton;

    /// <summary>
    /// Initializes a new instance of the ReportsForm class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ReportsForm(WarehouseContext context)
    {
        _context = context;
        InitializeComponent();
        InitializeControls();
        LoadProducts();
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // ReportsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Name = "ReportsForm";
        Text = "Reports";
        ResumeLayout(false);
    }

    private void InitializeControls()
    {
        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 20,
            BackColor = Color.FromArgb(240, 240, 240)
        };

        _gridView = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            Margin = new Padding(10),
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            RowHeadersVisible = false
        };

        _productFilter = new ComboBox
        {
            Size = new Size(200, 30),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Margin = new Padding(5)
        };

        _dateFrom = new DateTimePicker
        {
            Size = new Size(150, 30),
            Format = DateTimePickerFormat.Short,
            Margin = new Padding(5)
        };

        _dateTo = new DateTimePicker
        {
            Size = new Size(150, 30),
            Format = DateTimePickerFormat.Short,
            Margin = new Padding(5)
        };

        _generateButton = new Button
        {
            Text = "Generate Report",
            Size = new Size(120, 35),
            Margin = new Padding(5)
        };
        _generateButton.Click += GenerateButton_Click;

        var filterPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 60,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10),
            BackColor = Color.FromArgb(240, 240, 240)
        };
        filterPanel.Controls.AddRange(new Control[] { _productFilter, _dateFrom, _dateTo, _generateButton });

        _exportButton = new Button
        {
            Text = "Export to PDF",
            Size = new Size(120, 35),
            Margin = new Padding(5)
        };
        _exportButton.Click += ExportButton_Click;

        _printButton = new Button
        {
            Text = "Print",
            Size = new Size(120, 35),
            Margin = new Padding(5)
        };
        _printButton.Click += PrintButton_Click;

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10),
            BackColor = Color.FromArgb(240, 240, 240)
        };
        buttonPanel.Controls.AddRange(new Control[] { _exportButton, _printButton });

        Controls.Add(_gridView);
        Controls.Add(filterPanel);
        Controls.Add(buttonPanel);
        Controls.Add(topPanel);
    }

    private void LoadProducts()
    {
        _context.Products.Load();
        _productFilter.DataSource = _context.Products.Local.ToBindingList();
        _productFilter.DisplayMember = "Name";
        _productFilter.ValueMember = "ProductID";
    }

    private void GenerateButton_Click(object sender, EventArgs e)
    {
        var query = _context.Transactions
            .Include(t => t.Product)
            .AsQueryable();

        if (_productFilter.SelectedValue != null)
        {
            var productId = (int)_productFilter.SelectedValue;
            query = query.Where(t => t.ProductID == productId);
        }

        query = query.Where(t => t.Date >= _dateFrom.Value.Date && t.Date <= _dateTo.Value.Date.AddDays(1));

        var transactions = query.OrderBy(t => t.Date).ToList();
        var reportData = new List<ReportItem>();

        foreach (var transaction in transactions)
        {
            reportData.Add(new ReportItem
            {
                Date = transaction.Date,
                ProductName = transaction.Product?.Name ?? "Unknown",
                Type = transaction.Type == 'I' ? "In" : "Out",
                Quantity = transaction.Quantity,
                Notes = transaction.Notes
            });
        }

        _gridView.DataSource = reportData;
    }

    private void ExportButton_Click(object sender, EventArgs e)
    {
        if (_gridView.Rows.Count == 0)
        {
            MessageBox.Show("No data to export.", "Export Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var saveDialog = new SaveFileDialog
        {
            Filter = "PDF files (*.pdf)|*.pdf",
            Title = "Export to PDF",
            FileName = $"Report_{DateTime.Now:yyyyMMdd}.pdf"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            var document = new Document(PageSize.A4, 50, 50, 50, 50);
            var writer = PdfWriter.GetInstance(document, new FileStream(saveDialog.FileName, FileMode.Create));

            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            document.Add(new Paragraph("Warehouse Management System Report", titleFont));
            document.Add(new Paragraph($"Generated on: {DateTime.Now:g}", normalFont));
            document.Add(new Paragraph($"Period: {_dateFrom.Value:d} to {_dateTo.Value:d}", normalFont));
            document.Add(new Paragraph("\n"));

            var table = new PdfPTable(_gridView.Columns.Count);
            table.WidthPercentage = 100;

            // Add headers
            foreach (DataGridViewColumn column in _gridView.Columns)
            {
                table.AddCell(new PdfPCell(new Phrase(column.HeaderText, headerFont)));
            }

            // Add data
            foreach (DataGridViewRow row in _gridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new PdfPCell(new Phrase(cell.Value?.ToString() ?? "", normalFont)));
                }
            }

            document.Add(table);
            document.Close();

            MessageBox.Show("Report exported successfully.", "Export Complete",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void PrintButton_Click(object sender, EventArgs e)
    {
        if (_gridView.Rows.Count == 0)
        {
            MessageBox.Show("No data to print.", "Print Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var printDocument = new PrintDocument();
        printDocument.PrintPage += PrintDocument_PrintPage;

        using var printDialog = new PrintDialog
        {
            Document = printDocument
        };

        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            printDocument.Print();
        }
    }

    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
        var graphics = e.Graphics;
        var font = new System.Drawing.Font("Arial", 10);
        var headerFont = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
        var yPos = 50;
        var leftMargin = e.MarginBounds.Left;
        var topMargin = e.MarginBounds.Top;

        // Print title
        graphics.DrawString("Warehouse Management System Report", headerFont, Brushes.Black, leftMargin, yPos);
        yPos += 30;

        // Print date range
        graphics.DrawString($"Period: {_dateFrom.Value:d} to {_dateTo.Value:d}", font, Brushes.Black, leftMargin, yPos);
        yPos += 30;

        // Print headers
        var xPos = leftMargin;
        foreach (DataGridViewColumn column in _gridView.Columns)
        {
            graphics.DrawString(column.HeaderText, headerFont, Brushes.Black, xPos, yPos);
            xPos += column.Width;
        }
        yPos += 20;

        // Print data
        foreach (DataGridViewRow row in _gridView.Rows)
        {
            xPos = leftMargin;
            foreach (DataGridViewCell cell in row.Cells)
            {
                graphics.DrawString(cell.Value?.ToString() ?? "", font, Brushes.Black, xPos, yPos);
                xPos += _gridView.Columns[cell.ColumnIndex].Width;
            }
            yPos += 20;

            if (yPos > e.MarginBounds.Bottom)
            {
                e.HasMorePages = true;
                return;
            }
        }
    }

    private class ReportItem
    {
        public DateTime Date { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string? Notes { get; set; }
    }
} 