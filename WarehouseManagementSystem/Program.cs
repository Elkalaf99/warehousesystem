using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Forms;

namespace WarehouseManagementSystem;

/// <summary>
/// The main entry point for the application.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            using var serviceProvider = services.BuildServiceProvider();
            
            // Ensure database is created
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WarehouseContext>();
            context.Database.EnsureCreated();

            var mainForm = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"An error occurred while starting the application: {ex.Message}\n\nPlease check your database settings.",
                "Startup Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "warehouse.db");
        services.AddDbContext<WarehouseContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
        });

        services.AddSingleton<MainForm>();
        services.AddTransient<ProductsForm>();
        services.AddTransient<TransactionsForm>();
        services.AddTransient<ReportsForm>();
        services.AddTransient<SettingsForm>();
    }
} 