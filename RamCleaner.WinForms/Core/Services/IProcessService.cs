namespace RamCleaner.WinForms.Core.Services
{
    public record ProcessInfo(int Id, string Name, long MemoryUsageBytes, string MemoryUsageDisplay);

    public interface IProcessService
    {
        Task<IReadOnlyList<ProcessInfo>> GetHighUsageProcessesAsync(long thresholdBytes, CancellationToken ct = default);
    }
}
