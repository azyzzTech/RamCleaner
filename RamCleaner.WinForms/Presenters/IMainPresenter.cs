using RamCleaner.WinForms.Core.Services;

namespace RamCleaner.WinForms.Presenters
{
    public interface IMainPresenter
    {
        Task<IReadOnlyList<ProcessInfo>> GetHighUsageProcessesAsync(long thresholdBytes, CancellationToken ct = default);
        Task CleanProcessesAsync(IEnumerable<string> processNames, CancellationToken ct = default);
        bool IsStartupEnabled();
        void EnableStartup();
        void DisableStartup();
    }
}
