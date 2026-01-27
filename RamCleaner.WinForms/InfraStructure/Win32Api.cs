using System.Runtime.InteropServices;

namespace RamCleaner.WinForms.InfraStructure;

public static class Win32Api
{
    [DllImport("psapi.dll", SetLastError = true)]
    public static extern int EmptyWorkingSet(IntPtr hwProc);
}
