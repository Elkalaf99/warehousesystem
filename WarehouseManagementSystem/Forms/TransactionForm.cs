using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagementSystem.Forms;

/// <summary>
/// Form for adding a new transaction.
/// </summary>
public partial class TransactionForm : Form
{
    private WarehouseContext _context;
    private ComboBox _productComboBox;
    private NumericUpDown _quantityNumeric;
    private ComboBox _typeComboBox;
    private DateTimePicker _datePicker;
    private TextBox _notesTextBox;
    private Button _saveButton;
    private Button _cancelButton;

    /// <summary>
    /// Gets the transaction being created.
    /// </summary>
    public Transaction Transaction { get; private set; }

    /// <summary>
    /// Initializes a new instance of the TransactionForm class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TransactionForm(WarehouseContext context)
    {
        _context = context;
        Transaction = new Transaction { Date = DateTime.Now };
        InitializeComponent();
        InitializeControls();
        LoadProducts();
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // TransactionForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(500, 400);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "TransactionForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "New Transaction";
        ResumeLayout(false);
    }

    private void InitializeControls()
    {
        var productLabel = new Label
        {
            Text = "Product:",
            Location = new Point(20, 25),
            AutoSize = true
        };

        _productComboBox = new ComboBox
        {
            Location = new Point(150, 22),
            Size = new Size(300, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        var quantityLabel = new Label
        {
            Text = "Quantity:",
            Location = new Point(20, 65),
            AutoSize = true
        };

        _quantityNumeric = new NumericUpDown
        {
            Location = new Point(150, 63),
            Size = new Size(150, 23),
            DecimalPlaces = 2,
            Maximum = 999999,
            Minimum = 0.01m
        };

        var typeLabel = new Label
        {
            Text = "Type:",
            Location = new Point(20, 105),
            AutoSize = true
        };

        _typeComboBox = new ComboBox
        {
            Location = new Point(150, 103),
            Size = new Size(150, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _typeComboBox.Items.AddRange(new object[] { "In", "Out" });
        _typeComboBox.SelectedIndex = 0;

        var dateLabel = new Label
        {
            Text = "Date:",
            Location = new Point(20, 145),
            AutoSize = true
        };

        _datePicker = new DateTimePicker
        {
            Location = new Point(150, 143),
            Size = new Size(200, 23),
            Format = DateTimePickerFormat.Short
        };

        var notesLabel = new Label
        {
            Text = "Notes:",
            Location = new Point(20, 185),
            AutoSize = true
        };

        _notesTextBox = new TextBox
        {
            Location = new Point(150, 183),
            Size = new Size(300, 100),
            Multiline = true
        };

        _saveButton = new Button
        {
            DialogResult = DialogResult.OK,
            Location = new Point(150, 320),
            Size = new Size(100, 30),
            Text = "Save"
        };
        _saveButton.Click += SaveButton_Click;

        _cancelButton = new Button
        {
            DialogResult = DialogResult.Cancel,
            Location = new Point(270, 320),
            Size = new Size(100, 30),
            Text = "Cancel"
        };

        Controls.AddRange(new Control[]
        {
            productLabel,
            _productComboBox,
            quantityLabel,
            _quantityNumeric,
            typeLabel,
            _typeComboBox,
            dateLabel,
            _datePicker,
            notesLabel,
            _notesTextBox,
            _saveButton,
            _cancelButton
        });
    }

    private void LoadProducts()
    {
        _context.Products.Load();
        _productComboBox.DataSource = _context.Products.Local.ToBindingList();
        _productComboBox.DisplayMember = "Name";
        _productComboBox.ValueMember = "ProductID";
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        if (_productComboBox.SelectedValue == null)
        {
            MessageBox.Show("Please select a product.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            return;
        }

        Transaction.ProductID = (int)_productComboBox.SelectedValue;
        Transaction.Quantity = _quantityNumeric.Value;
        Transaction.Type = _typeComboBox.SelectedIndex == 0 ? 'I' : 'O';
        Transaction.Date = _datePicker.Value;
        Transaction.Notes = _notesTextBox.Text;
    }
} 