using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Forms;

/// <summary>
/// Form for managing transactions in the warehouse.
/// </summary>
public partial class TransactionsForm : Form
{
    private WarehouseContext _context;
    private BindingSource _bindingSource;
    private DataGridView _gridView;
    private Button _addButton;
    private ComboBox _productFilter;
    private DateTimePicker _dateFrom;
    private DateTimePicker _dateTo;
    private Button _filterButton;

    /// <summary>
    /// Initializes a new instance of the TransactionsForm class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TransactionsForm(WarehouseContext context)
    {
        _context = context;
        _bindingSource = new BindingSource();

        InitializeComponent();
        InitializeControls();
        LoadData();
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // TransactionsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Name = "TransactionsForm";
        Text = "Transactions";
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

        _addButton = new Button
        {
            Text = "Add Transaction",
            Size = new Size(150, 35),
            Margin = new Padding(5)
        };
        _addButton.Click += AddButton_Click;

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

        _filterButton = new Button
        {
            Text = "Apply Filter",
            Size = new Size(120, 35),
            Margin = new Padding(5)
        };
        _filterButton.Click += FilterButton_Click;

        var filterPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 60,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10),
            BackColor = Color.FromArgb(240, 240, 240)
        };
        filterPanel.Controls.AddRange(new Control[] { _productFilter, _dateFrom, _dateTo, _filterButton });

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10),
            BackColor = Color.FromArgb(240, 240, 240)
        };
        buttonPanel.Controls.Add(_addButton);

        Controls.Add(_gridView);
        Controls.Add(filterPanel);
        Controls.Add(buttonPanel);
        Controls.Add(topPanel);
    }

    private void LoadData()
    {
        _context.Products.Load();
        _context.Transactions.Load();

        _productFilter.DataSource = _context.Products.Local.ToBindingList();
        _productFilter.DisplayMember = "Name";
        _productFilter.ValueMember = "ProductID";

        _bindingSource.DataSource = _context.Transactions.Local.ToBindingList();
        _gridView.DataSource = _bindingSource;
    }

    private void AddButton_Click(object sender, EventArgs e)
    {
        using var form = new TransactionForm(_context);
        if (form.ShowDialog() == DialogResult.OK)
        {
            _context.Transactions.Add(form.Transaction);
            _context.SaveChanges();
        }
    }

    private void FilterButton_Click(object sender, EventArgs e)
    {
        var query = _context.Transactions.AsQueryable();

        if (_productFilter.SelectedValue != null)
        {
            var productId = (int)_productFilter.SelectedValue;
            query = query.Where(t => t.ProductID == productId);
        }

        query = query.Where(t => t.Date >= _dateFrom.Value.Date && t.Date <= _dateTo.Value.Date.AddDays(1));

        _bindingSource.DataSource = query.ToList();
    }
} 