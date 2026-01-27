using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace RamCleaner.WinForms.Core.Services
{
    public interface ILocalizationService
    {
        IEnumerable<CultureInfo> GetAvailableCultures();
        void SetCulture(CultureInfo culture);
        void ApplyResources(Form form);
    }
}
