using System.Threading.Tasks;

namespace RamCleaner.WinForms.Core.Services
{
    public interface IStartupService
    {
        bool IsStartupEnabled();
        void EnableStartup();
        void DisableStartup();
    }
}
