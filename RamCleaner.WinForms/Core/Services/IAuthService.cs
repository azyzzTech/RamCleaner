using System.Threading;
using System.Threading.Tasks;

namespace RamCleaner.WinForms.Core.Services
{
    public interface IAuthService
    {
        Task<bool> FullAuthFlowAsync(CancellationToken ct = default);
    }
}
