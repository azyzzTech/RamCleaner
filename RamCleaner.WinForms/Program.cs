using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RamCleaner.WinForms.Business;
using RamCleaner.WinForms.Core.Services;
using RamCleaner.WinForms.Forms;
using RamCleaner.WinForms.InfraStructure;
using RamCleaner.WinForms.Services;

namespace RamCleaner.WinForms;

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
                services.AddSingleton<IProcessService, ProcessBusiness>();
                services.AddSingleton<IRamCleanerService, RamBusiness>();
                services.AddSingleton<IStartupService, StartupService>();
                services.AddSingleton<IAuthService, DiscordAuth>();
                services.AddTransient<LoginForm>();
                services.AddTransient<MainForm>();
            })
            .Build();

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

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
