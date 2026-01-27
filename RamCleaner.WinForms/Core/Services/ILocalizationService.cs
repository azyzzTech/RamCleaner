using System.Globalization;

namespace RamCleaner.WinForms.Core.Services
{
    public interface ILocalizationService
    {
        IEnumerable<CultureInfo> GetAvailableCultures();
        void SetCulture(CultureInfo culture);
        void ApplyResources(Form form);
    }
}
