using Microsoft.Extensions.Logging;
using RamCleaner.WinForms.Core.Services;
using RamCleaner.WinForms.InfraStructure;
using System.Diagnostics;

namespace RamCleaner.WinForms.Business;

internal class RamBusiness : IRamCleanerService
{
    private readonly ILogger<RamBusiness>? _logger;

    public RamBusiness(ILogger<RamBusiness>? logger = null)
    {
        _logger = logger;
    }

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
