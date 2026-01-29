namespace RamCleaner.WinForms.Core.Services
{
    public interface IAuthService
    {
        Task<bool> FullAuthFlowAsync(CancellationToken ct = default);
        /// <summary>
        /// Checks whether the current stored authorization is valid (e.g., within 1 week)
        /// without triggering an interactive auth flow.
        /// </summary>
        Task<bool> ValidateAuthStatusAsync(CancellationToken ct = default);
    }
}
