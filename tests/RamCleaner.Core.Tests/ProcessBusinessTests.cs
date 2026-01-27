using System.Threading;
using System.Threading.Tasks;
using Xunit;
using RamCleaner.WinForms.Business;
using RamCleaner.WinForms.Core.Services;
using System.Collections.Generic;

namespace RamCleaner.Core.Tests
{
    public class ProcessBusinessTests
    {
        [Fact]
        public async Task GetHighUsageProcessesAsync_ReturnsList()
        {
            var service = new ProcessBusiness();
            long threshold = 0; // everything should be returned

            var list = await service.GetHighUsageProcessesAsync(threshold, CancellationToken.None);

            Assert.NotNull(list);
            Assert.IsType<List<RamCleaner.WinForms.Core.Services.ProcessInfo>>(list);
        }
    }
}
