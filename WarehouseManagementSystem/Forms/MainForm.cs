using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagementSystem.Forms;

namespace WarehouseManagementSystem;

/// <summary>
/// The main form of the warehouse management system.
/// </summary>
public partial class MainForm : Form
{
    private readonly Panel _contentPanel;
    private readonly MenuStrip _menuStrip;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the MainForm class.
    /// </summary>
    public MainForm(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        InitializeMainForm();
        
        _contentPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10)
        };

        _menuStrip = new MenuStrip();
        InitializeMenuStrip();

        Controls.Add(_menuStrip);
        Controls.Add(_contentPanel);
    }

    private void InitializeMainForm()
    {
        Text = "Warehouse Management System";
        Size = new Size(1200, 800);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1000, 700);
    }

    private void InitializeMenuStrip()
    {
        _menuStrip.Height = 30;  // Increase menu height
        _menuStrip.Padding = new Padding(5);

        var productsMenu = new ToolStripMenuItem("Products");
        productsMenu.Click += (s, e) => LoadForm(_serviceProvider.GetRequiredService<ProductsForm>());

        var transactionsMenu = new ToolStripMenuItem("Transactions");
        transactionsMenu.Click += (s, e) => LoadForm(_serviceProvider.GetRequiredService<TransactionsForm>());

        var reportsMenu = new ToolStripMenuItem("Reports");
        reportsMenu.Click += (s, e) => LoadForm(_serviceProvider.GetRequiredService<ReportsForm>());

        var settingsMenu = new ToolStripMenuItem("Settings");
        settingsMenu.Click += (s, e) => LoadForm(_serviceProvider.GetRequiredService<SettingsForm>());

        _menuStrip.Items.AddRange(new ToolStripItem[]
        {
            productsMenu,
            transactionsMenu,
            reportsMenu,
            settingsMenu
        });
    }

    private void LoadForm(Form form)
    {
        _contentPanel.Controls.Clear();
        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        _contentPanel.Controls.Add(form);
        form.Show();
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1024, 768);
        Name = "MainForm";
        Text = "Warehouse Management System";
        ResumeLayout(false);
    }
} 