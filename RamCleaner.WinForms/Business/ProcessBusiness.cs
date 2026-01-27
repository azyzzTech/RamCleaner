using Microsoft.Extensions.Logging;
using RamCleaner.WinForms.Core.Services;
using System.Diagnostics;

namespace RamCleaner.WinForms.Business;

/// <summary>
/// Service responsible for enumerating system processes and returning
/// processes that exceed a given memory threshold.
/// </summary>
internal class ProcessBusiness : IProcessService
{
    private readonly ILogger<ProcessBusiness>? _logger;

    /// <summary>
    /// Creates a new instance of <see cref="ProcessBusiness"/>.
    /// </summary>
    public ProcessBusiness(ILogger<ProcessBusiness>? logger = null)
    {
        _logger = logger;
    }

    /// <summary>
    /// Returns a list of processes whose working set exceeds the specified threshold.
    /// Observes cancellation via <paramref name="ct"/>.
    /// </summary>
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

    /// <summary>
    /// Formats a byte value into a human-readable string (B/KB/MB/GB).
    /// </summary>
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
