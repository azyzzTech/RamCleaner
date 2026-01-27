namespace RamCleaner.WinForms.Core.Services
{
    public interface IStartupService
    {
        bool IsStartupEnabled();
        void EnableStartup();
        void DisableStartup();
    }
}
