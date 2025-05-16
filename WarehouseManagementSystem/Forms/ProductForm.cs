using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Forms;

/// <summary>
/// Form for adding or editing a product.
/// </summary>
public partial class ProductForm : Form
{
    private TextBox _nameTextBox;
    private NumericUpDown _balanceNumeric;
    private Button _saveButton;
    private Button _cancelButton;

    /// <summary>
    /// Gets the product being edited.
    /// </summary>
    public Product Product { get; private set; }

    /// <summary>
    /// Initializes a new instance of the ProductForm class for adding a new product.
    /// </summary>
    public ProductForm()
    {
        Product = new Product();
        InitializeComponent();
        InitializeControls();
    }

    /// <summary>
    /// Initializes a new instance of the ProductForm class for editing an existing product.
    /// </summary>
    /// <param name="product">The product to edit.</param>
    public ProductForm(Product product)
    {
        Product = product;
        InitializeComponent();
        InitializeControls();
        LoadProductData();
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // ProductForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(500, 300);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ProductForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Product";
        ResumeLayout(false);
    }

    private void InitializeControls()
    {
        var nameLabel = new Label
        {
            Text = "Name:",
            Location = new Point(20, 25),
            AutoSize = true
        };

        _nameTextBox = new TextBox
        {
            Location = new Point(150, 22),
            Size = new Size(300, 23)
        };

        var balanceLabel = new Label
        {
            Text = "Beginning Balance:",
            Location = new Point(20, 65),
            AutoSize = true
        };

        _balanceNumeric = new NumericUpDown
        {
            Location = new Point(150, 63),
            Size = new Size(150, 23),
            DecimalPlaces = 2,
            Maximum = 999999,
            Minimum = 0
        };

        _saveButton = new Button
        {
            DialogResult = DialogResult.OK,
            Location = new Point(150, 220),
            Size = new Size(100, 30),
            Text = "Save"
        };
        _saveButton.Click += SaveButton_Click;

        _cancelButton = new Button
        {
            DialogResult = DialogResult.Cancel,
            Location = new Point(270, 220),
            Size = new Size(100, 30),
            Text = "Cancel"
        };

        Controls.AddRange(new Control[]
        {
            nameLabel,
            _nameTextBox,
            balanceLabel,
            _balanceNumeric,
            _saveButton,
            _cancelButton
        });
    }

    private void LoadProductData()
    {
        _nameTextBox.Text = Product.Name;
        _balanceNumeric.Value = Product.BeginningBalance;
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
        {
            MessageBox.Show("Please enter a product name.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            return;
        }

        Product.Name = _nameTextBox.Text;
        Product.BeginningBalance = _balanceNumeric.Value;
    }
} 