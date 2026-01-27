using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RamCleaner.WinForms.Business;
using RamCleaner.WinForms.Core.Services;
using RamCleaner.WinForms.Forms;
using RamCleaner.WinForms.InfraStructure;
using RamCleaner.WinForms.Services;

namespace RamCleaner.WinForms;

/// <summary>
/// Application entry point and host configuration.
/// Configures dependency injection and starts the WinForms application.
/// </summary>
internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();
                // Register IHttpClientFactory and related helpers
                services.AddHttpClient();
                services.AddSingleton<IProcessService, ProcessBusiness>();
                services.AddSingleton<IRamCleanerService, RamBusiness>();
                services.AddSingleton<IStartupService, StartupService>();
                services.AddSingleton<IAuthService, DiscordAuth>();
                services.AddSingleton<ILocalizationService, LocalizationService>();
                services.AddTransient<LoginForm>();
                services.AddTransient<MainForm>();
                services.AddTransient<RamCleaner.WinForms.Presenters.IMainPresenter, RamCleaner.WinForms.Presenters.MainPresenter>();
            })
            .Build();

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        // Restore saved culture before creating forms (guard in case setting doesn't exist)
        try
        {
            var savedCulture = RamCleaner.WinForms.Properties.Settings.Default["UICulture"] as string;
            if (!string.IsNullOrEmpty(savedCulture))
            {
                var culture = new System.Globalization.CultureInfo(savedCulture);
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                RamCleaner.WinForms.Properties.Resources.Culture = culture;
            }
        }
        catch (System.Configuration.SettingsPropertyNotFoundException)
        {
            // setting not present; ignore
        }

        var login = services.GetRequiredService<LoginForm>();

        if (login.ShowDialog() == DialogResult.OK)
        {
            var main = services.GetRequiredService<MainForm>();
            Application.Run(main);
        }
        else
        {
            Application.Exit();
        }
    }
}
