using Microsoft.Extensions.Logging;
using RamCleaner.WinForms.Core.Services;
using RamCleaner.WinForms.InfraStructure;
using System.Diagnostics;

namespace RamCleaner.WinForms.Business;

/// <summary>
/// Service that performs memory cleaning for processes using the Win32 API.
/// Iterates system processes and calls EmptyWorkingSet for matching processes.
/// </summary>
internal class RamBusiness : IRamCleanerService
{
    private readonly ILogger<RamBusiness>? _logger;

    /// <summary>
    /// Creates a new instance of <see cref="RamBusiness"/>.
    /// </summary>
    public RamBusiness(ILogger<RamBusiness>? logger = null)
    {
        _logger = logger;
    }

    /// <summary>
    /// Cleans memory for the provided process names. If <paramref name="processNames"/> is null,
    /// attempts to clean all processes.
    /// </summary>
    public Task CleanMemoryAsync(IEnumerable<string>? processNames = null, CancellationToken ct = default)
    {
        return Task.Run(() =>
        {
            var processes = Process.GetProcesses();

            foreach (var proc in processes)
            {
                if (ct.IsCancellationRequested)
                    break;

                try
                {
                    if (processNames == null || processNames.Contains(proc.ProcessName))
                    {
                        Win32Api.EmptyWorkingSet(proc.Handle);
                        _logger?.LogDebug("Cleaned process {ProcessName} (PID {Pid})", proc.ProcessName, proc.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogDebug(ex, "Failed to clean process {ProcessName}", proc.ProcessName);
                }
                finally
                {
                    try { proc.Dispose(); } catch (Exception ex) { _logger?.LogDebug(ex, "Failed disposing process"); }
                }
            }
        }, ct);
    }
}
