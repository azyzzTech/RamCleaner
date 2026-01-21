using System.Runtime.InteropServices;

namespace RamCleaner.WinForms.InfraStructure;

public static class Win32Api
{
    [DllImport("psapi.dll")]
    public static extern int EmptyWorkingSet(nint hwProc);
}