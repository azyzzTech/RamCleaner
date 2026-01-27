using RamCleaner.WinForms.Core.Services;
using RamCleaner.WinForms.InfraStructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RamCleaner.WinForms.Business;

internal class RamBusiness : IRamCleanerService
{
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
                    }
                }
                catch { }
                finally
                {
                    try { proc.Dispose(); } catch { }
                }
            }
        }, ct);
    }
}
