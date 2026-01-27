using Microsoft.Extensions.Logging;
using RamCleaner.WinForms.Core.Models;
using RamCleaner.WinForms.Core.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RamCleaner.WinForms.Business;

internal class ProcessBusiness : IProcessService
{
    private readonly ILogger<ProcessBusiness>? _logger;

    public ProcessBusiness(ILogger<ProcessBusiness>? logger = null)
    {
        _logger = logger;
    }

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
                        string display = FormatBytes(usage);
                        processList.Add(new ProcessInfo(proc.Id, proc.ProcessName, usage, display));
                    }
                }
                catch (Exception ex)
                {
                    // Process may exit or be inaccessible; log and continue
                    _logger?.LogDebug(ex, "Ignored process while enumerating");
                }
                finally
                {
                    try { proc.Dispose(); } catch (Exception ex) { _logger?.LogDebug(ex, "Failed disposing process"); }
                }
            }

            return processList.OrderByDescending(p => p.MemoryUsageBytes).ToList() as System.Collections.Generic.IReadOnlyList<ProcessInfo>;
        }, ct);
    }

    private static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}
