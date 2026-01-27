using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace RamCleaner.WinForms.Core.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ResourceManager _resourceManager;

        /// <summary>
        /// Service for applying cultures to forms and discovering available cultures.
        /// </summary>
        public LocalizationService()
        {
            _resourceManager = RamCleaner.WinForms.Properties.Resources.ResourceManager;
        }

        public IEnumerable<CultureInfo> GetAvailableCultures()
        {
            // Return cultures for which resource exists. We include en-US and tr-TR by default.
            return new[] { new CultureInfo("en-US"), new CultureInfo("tr-TR") };
        }

        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            RamCleaner.WinForms.Properties.Resources.Culture = culture;
        }

        public void ApplyResources(Form form)
        {
            var rm = new ComponentResourceManager(form.GetType());
            rm.ApplyResources(form, "$this");
            ApplyResourcesRecursively(rm, form);
        }

        private void ApplyResourcesRecursively(ComponentResourceManager rm, Control ctrl)
        {
            foreach (Control child in ctrl.Controls)
            {
                if (!string.IsNullOrEmpty(child.Name))
                    rm.ApplyResources(child, child.Name);

                if (child.HasChildren)
                    ApplyResourcesRecursively(rm, child);
            }

            if (ctrl is Form f)
            {
                foreach (ToolStripItem item in GetAllToolStripItems(f))
                {
                    if (!string.IsNullOrEmpty(item.Name))
                        rm.ApplyResources(item, item.Name);
                }
            }
        }

        private IEnumerable<ToolStripItem> GetAllToolStripItems(Control root)
        {
            var list = new List<ToolStripItem>();
            foreach (Control c in root.Controls)
            {
                if (c is ToolStrip ts)
                    list.AddRange(ts.Items.Cast<ToolStripItem>());
                if (c.HasChildren)
                    list.AddRange(GetAllToolStripItems(c));
            }
            return list;
        }
    }
}
