using System.ComponentModel;
using System.Data;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Forms;

/// <summary>
/// Form for managing products in the warehouse.
/// </summary>
public partial class ProductsForm : Form
{
    private WarehouseContext _context;
    private BindingSource _bindingSource;
    private DataGridView _gridView;
    private Button _addButton;
    private Button _editButton;
    private Button _deleteButton;

    /// <summary>
    /// Initializes a new instance of the ProductsForm class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProductsForm(WarehouseContext context)
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
        // ProductsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Name = "ProductsForm";
        Text = "Products Management";
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
            Text = "Add",
            Size = new Size(120, 35),
            Margin = new Padding(5)
        };
        _addButton.Click += AddButton_Click;

        _editButton = new Button
        {
            Text = "Edit",
            Size = new Size(120, 35),
            Margin = new Padding(5)
        };
        _editButton.Click += EditButton_Click;

        _deleteButton = new Button
        {
            Text = "Delete",
            Size = new Size(120, 35),
            Margin = new Padding(5)
        };
        _deleteButton.Click += DeleteButton_Click;

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10),
            BackColor = Color.FromArgb(240, 240, 240)
        };
        buttonPanel.Controls.AddRange(new Control[] { _addButton, _editButton, _deleteButton });

        Controls.Add(_gridView);
        Controls.Add(buttonPanel);
        Controls.Add(topPanel);
    }

    private void LoadData()
    {
        _context.Products.Load();
        _bindingSource.DataSource = _context.Products.Local.ToBindingList();
        _gridView.DataSource = _bindingSource;
    }

    private void AddButton_Click(object sender, EventArgs e)
    {
        using var form = new ProductForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            _context.Products.Add(form.Product);
            _context.SaveChanges();
        }
    }

    private void EditButton_Click(object sender, EventArgs e)
    {
        if (_gridView.SelectedRows.Count == 0) return;

        var product = (Product)_gridView.SelectedRows[0].DataBoundItem;
        using var form = new ProductForm(product);
        if (form.ShowDialog() == DialogResult.OK)
        {
            _context.SaveChanges();
        }
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        if (_gridView.SelectedRows.Count == 0) return;

        var product = (Product)_gridView.SelectedRows[0].DataBoundItem;
        if (MessageBox.Show(
            $"Are you sure you want to delete {product.Name}?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
} 