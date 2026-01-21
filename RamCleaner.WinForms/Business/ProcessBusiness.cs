using RamCleaner.WinForms.Core.Models;
using System.Diagnostics;

namespace RamCleaner.WinForms.Business;

internal class ProcessBusiness
{
    internal List<ProcessModel> GetHighUsageProcesses(long? customThreshold = null)
    {
        var processList = new List<ProcessModel>();
        long threshold = customThreshold ?? 500L * 1024 * 1024;

        var allProcesses = Process.GetProcesses();

        foreach (var proc in allProcesses)
        {
            try
            {
                long usage = proc.WorkingSet64;

                if (usage > threshold)
                {
                    processList.Add(new ProcessModel
                    {
                        Id = proc.Id,
                        Name = proc.ProcessName,
                        MemoryUsageBytes = usage,
                        MemoryUsageDisplay = (usage / 1024 / 1024).ToString() + " MB",
                        IsSelected = true
                    });
                }
            }
            catch { }
        }

        return processList.OrderByDescending(p => p.MemoryUsageBytes).ToList();
    }
}