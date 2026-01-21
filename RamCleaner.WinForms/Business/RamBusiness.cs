using RamCleaner.WinForms.InfraStructure;
using System.Diagnostics;

namespace RamCleaner.WinForms.Business;

internal class RamBusiness
{
    internal void CleanMemory(IEnumerable<string> processNames = null)
    {
        var processes = Process.GetProcesses();

        foreach (var proc in processes)
        {
            try
            {
                if (processNames == null || processNames.Contains(proc.ProcessName))
                {
                    Win32Api.EmptyWorkingSet(proc.Handle);
                }
            }
            catch { }
        }
    }
}