using System;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WarehouseManagementSystem.Forms;

/// <summary>
/// Form for managing application settings.
/// </summary>
public partial class SettingsForm : Form
{
    private TextBox _companyNameTextBox;
    private Button _saveButton;
    private Button _cancelButton;

    /// <summary>
    /// Initializes a new instance of the SettingsForm class.
    /// </summary>
    public SettingsForm()
    {
        InitializeComponent();
        InitializeControls();
        LoadSettings();
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // SettingsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 150);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SettingsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Settings";
        ResumeLayout(false);
    }

    private void InitializeControls()
    {
        // Company Name
        var companyNameLabel = new Label
        {
            Text = "Company Name:",
            Location = new System.Drawing.Point(20, 20),
            AutoSize = true
        };

        _companyNameTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, 20),
            Width = 250
        };

        // Buttons
        _saveButton = new Button
        {
            Text = "Save",
            Location = new System.Drawing.Point(120, 60),
            Width = 100
        };
        _saveButton.Click += SaveButton_Click;

        _cancelButton = new Button
        {
            Text = "Cancel",
            Location = new System.Drawing.Point(230, 60),
            Width = 100
        };
        _cancelButton.Click += (s, e) => Close();

        // Add controls to form
        Controls.AddRange(new Control[] { 
            companyNameLabel, 
            _companyNameTextBox,
            _saveButton,
            _cancelButton
        });
    }

    private void LoadSettings()
    {
        try
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<AppSettings>(json);
                if (config != null)
                {
                    _companyNameTextBox.Text = config.CompanyName ?? string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            var config = new AppSettings
            {
                CompanyName = _companyNameTextBox.Text.Trim()
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(configPath, json);

            MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

public class AppSettings
{
    [JsonPropertyName("CompanyName")]
    public string? CompanyName { get; set; }
} 