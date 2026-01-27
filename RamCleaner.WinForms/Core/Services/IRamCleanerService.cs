namespace RamCleaner.WinForms.Core.Services
{
    public interface IRamCleanerService
    {
        Task CleanMemoryAsync(IEnumerable<string>? processNames = null, CancellationToken ct = default);
    }
}
