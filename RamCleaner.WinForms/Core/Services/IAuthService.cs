namespace RamCleaner.WinForms.Core.Services
{
    public interface IAuthService
    {
        Task<bool> FullAuthFlowAsync(CancellationToken ct = default);
    }
}
