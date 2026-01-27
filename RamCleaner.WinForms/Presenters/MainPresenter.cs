using Microsoft.Extensions.Logging;
using RamCleaner.WinForms.Core.Services;

namespace RamCleaner.WinForms.Presenters
{
    public class MainPresenter : IMainPresenter
    {
        private readonly IProcessService _processService;
        private readonly IRamCleanerService _ramCleanerService;
        private readonly IStartupService _startupService;
        private readonly ILogger<MainPresenter>? _logger;

        public MainPresenter(IProcessService processService, IRamCleanerService ramCleanerService, IStartupService startupService, ILogger<MainPresenter>? logger = null)
        {
            _processService = processService;
            _ramCleanerService = ramCleanerService;
            _startupService = startupService;
            _logger = logger;
        }

        public Task<IReadOnlyList<ProcessInfo>> GetHighUsageProcessesAsync(long thresholdBytes, CancellationToken ct = default)
        {
            return _processService.GetHighUsageProcessesAsync(thresholdBytes, ct);
        }

        public Task CleanProcessesAsync(IEnumerable<string> processNames, CancellationToken ct = default)
        {
            return _ramCleanerService.CleanMemoryAsync(processNames, ct);
        }

        public bool IsStartupEnabled() => _startupService.IsStartupEnabled();

        public void EnableStartup() => _startupService.EnableStartup();

        public void DisableStartup() => _startupService.DisableStartup();
    }
}
