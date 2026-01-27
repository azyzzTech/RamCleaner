using RamCleaner.WinForms.Core.Models;
using RamCleaner.WinForms.Core.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RamCleaner.WinForms.Business;

internal class ProcessBusiness : IProcessService
{
    public async Task<System.Collections.Generic.IReadOnlyList<ProcessInfo>> GetHighUsageProcessesAsync(long thresholdBytes, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            var processList = new System.Collections.Generic.List<ProcessInfo>();

            var allProcesses = Process.GetProcesses();

            foreach (var proc in allProcesses)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    long usage = proc.WorkingSet64;

                    if (usage > thresholdBytes)
                    {
                        processList.Add(new ProcessInfo(proc.Id, proc.ProcessName, usage, (usage / 1024 / 1024).ToString() + " MB"));
                    }
                }
                catch { }
                finally
                {
                    try { proc.Dispose(); } catch { }
                }
            }

            return processList.OrderByDescending(p => p.MemoryUsageBytes).ToList() as System.Collections.Generic.IReadOnlyList<ProcessInfo>;
        }, ct);
    }
}
