using System.Runtime.InteropServices;

namespace RamCleaner.WinForms.InfraStructure;

/// <summary>
/// P/Invoke declarations for native Windows APIs used by the application.
/// </summary>
public static class Win32Api
{
    [DllImport("psapi.dll", SetLastError = true)]
    public static extern int EmptyWorkingSet(IntPtr hwProc);
}
