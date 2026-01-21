using Microsoft.Win32;

namespace RamCleaner.WinForms.Services;

internal class StartupService
{
    private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string AppName = "RamCleaner";

    public static bool IsStartupEnabled()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
            {
                if (key != null)
                {
                    return key.GetValue(AppName) != null;
                }
            }
        }
        catch { }

        return false;
    }

    public static void EnableStartup()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    string executablePath = Application.ExecutablePath;
                    key.SetValue(AppName, $"\"{executablePath}\"");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to enable startup: {ex.Message}", ex);
        }
    }

    public static void DisableStartup()
    {
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    key.DeleteValue(AppName, false);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to disable startup: {ex.Message}", ex);
        }
    }
}