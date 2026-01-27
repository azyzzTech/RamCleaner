using Microsoft.Win32;
using RamCleaner.WinForms.Core.Services;
using System.Reflection;

namespace RamCleaner.WinForms.Services;

internal class StartupService : IStartupService
{
    private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "RamCleaner";

    public bool IsStartupEnabled()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
            {
                return key?.GetValue(AppName) != null;
            }
        }
        catch
        {
            return false;
        }
    }

    public void EnableStartup()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key == null) throw new InvalidOperationException("Unable to open registry key for startup.");

                string executablePath = Assembly.GetEntryAssembly()?.Location ?? throw new InvalidOperationException("Executable path could not be determined.");
                key.SetValue(AppName, $"\"{executablePath}\"");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to enable startup: {ex.Message}", ex);
        }
    }

    public void DisableStartup()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key == null) throw new InvalidOperationException("Unable to open registry key for startup.");
                key.DeleteValue(AppName, false);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to disable startup: {ex.Message}", ex);
        }
    }
}
