using Microsoft.VisualBasic.ApplicationServices;
using RamCleaner.WinForms.Business;
using RamCleaner.WinForms.Core.Services;
using RamCleaner.WinForms.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RamCleaner.WinForms.Forms;

internal partial class MainForm : Form
{
    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

    private System.Windows.Forms.Timer _timer = new();
    private System.Windows.Forms.Timer _refreshTimer = new();
    private bool _isClosing = false;
    // guard to prevent overlapping timer executions
    private int _isTickRunning = 0;
    private readonly IRamCleanerService _ramCleanerService;
    private readonly IProcessService _processService;
    private readonly IStartupService _startupService;
    private readonly ILogger<MainForm> _logger;
    private readonly ILocalizationService _localizationService;
    private System.Windows.Forms.ComboBox _cmbLanguage;

    public MainForm(IRamCleanerService ramCleanerService, IProcessService processService, IStartupService startupService, ILocalizationService localizationService, ILogger<MainForm> logger)
    {
        _ramCleanerService = ramCleanerService;
        _processService = processService;
        _startupService = startupService;
        // fallback if DI didn't provide localizationService (defensive)
        _localizationService = localizationService ?? new LocalizationService();
        _logger = logger;

        InitializeComponent();

        ApplyDarkThemeToListView();
        SetupListViewColumns();
        SetupLanguage();
        SetupTooltips();
        LoadStartupState();
        _ = LoadProcessesAsync();

        // create language selector combobox at runtime
        try
        {
            _cmbLanguage = new System.Windows.Forms.ComboBox();
            _cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cmbLanguage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _cmbLanguage.BackColor = Color.FromArgb(25, 25, 25);
            _cmbLanguage.ForeColor = Color.White;
            _cmbLanguage.FlatStyle = FlatStyle.Flat;
            _cmbLanguage.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _cmbLanguage.Size = new Size(184, 28);
            _cmbLanguage.Location = new Point(594, 154);

            var cultures = (_localizationService?.GetAvailableCultures() ?? new[] { new System.Globalization.CultureInfo("en-US") }).ToList();
            foreach (var c in cultures)
            {
                _cmbLanguage.Items.Add(new ComboBoxItem { Culture = c, DisplayName = c.NativeName });
            }

            // select current culture
            var current = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var idx = cultures.FindIndex(c => c.Name == current.Name);
            if (idx >= 0) _cmbLanguage.SelectedIndex = idx;

            _cmbLanguage.SelectedIndexChanged += CmbLanguage_SelectedIndexChanged;
            Controls.Add(_cmbLanguage);
        }
        catch { }

        _refreshTimer.Interval = 60000;
        _refreshTimer.Tick += async (s, e) =>
        {
            if (!chkAutoClean.Checked)
            {
                try { await LoadProcessesAsync(); } catch { }
            }
        };
        _refreshTimer.Start();

        notifyIcon.Icon = this.Icon;

        panelStats.Paint += (s, e) => DrawRedBorder(e.Graphics, panelStats);
        groupBoxActions.Paint += (s, e) => DrawRedBorder(e.Graphics, groupBoxActions);
        listViewProcesses.Paint += (s, e) => DrawRedBorder(e.Graphics, listViewProcesses);
    }

    private static void DrawRedBorder(Graphics g, Control control)
    {
        using Pen redPen = new(Color.Red, 1);
        g.DrawRectangle(redPen, 0, 0, control.Width - 1, control.Height - 1);
    }

    private void SetupListViewColumns()
    {
        int availableWidth = listViewProcesses.Width;
        ApplicationName.Width = (int)(availableWidth * 0.65);
        Memory.Width = availableWidth - ApplicationName.Width - 1;
    }

    private void ApplyDarkThemeToListView()
    {
        SetWindowTheme(listViewProcesses.Handle, "DarkMode_Explorer", null);
    }

    private void LoadStartupState()
    {
        try
        {
            chkStartup.Checked = _startupService.IsStartupEnabled();
        }
        catch
        {
            chkStartup.Checked = false;
        }
    }

    private void SetupLanguage()
    {
        // Use global resource strings for control texts/tooltips so they update when Resources.Culture changes
        this.Text = Properties.Resources.FormTitle;
        ApplicationName.Text = Properties.Resources.ApplicationName;
        btnClean.Text = Properties.Resources.BtnCleanText;
        btnRefresh.Text = Properties.Resources.BtnRefreshText;
        btnSelectAll.Text = Properties.Resources.BtnSelectAllText;
        btnSelectNone.Text = Properties.Resources.BtnSelectNoneText;
        chkAutoClean.Text = Properties.Resources.ChkAutoCleanText;
        chkStartup.Text = Properties.Resources.ChkStartupText;
        lblTitle.Text = Properties.Resources.LblTitle;
        Memory.Text = Properties.Resources.ColumnMemory;
        notifyIcon.Text = Properties.Resources.NotifyIconText;
        lnkYoutube.Text = Properties.Resources.LnkYoutubeText;
        groupBoxProcesses.Text = Properties.Resources.GroupBoxProcessesText;
        showToolStripMenuItem.Text = Properties.Resources.ShowMenuText;
        groupBoxActions.Text = Properties.Resources.GroupBoxActionsText;
        exitToolStripMenuItem.Text = Properties.Resources.ExitMenuText;
        label1.Text = Properties.Resources.LabelInterval;
        label2.Text = Properties.Resources.LabelProcessesFound;
        label3.Text = Properties.Resources.LabelTotalMemory;
        label4.Text = Properties.Resources.LabelRamThreshold;
        // update language combobox label if present
        if (_cmbLanguage != null)
        {
            // no direct text on combobox, items show native names
        }

        // Update tooltips and runtime labels
        SetupTooltips();
        UpdateRamThresholdInfo();
        UpdateIntervalInfo();
    }
    

    private void CmbLanguage_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (sender is System.Windows.Forms.ComboBox cb && cb.SelectedItem is ComboBoxItem item)
        {
            _localizationService.SetCulture(item.Culture);
            _localizationService.ApplyResources(this);
            SetupLanguage();
            try
            {
                var settings = RamCleaner.WinForms.Properties.Settings.Default;
                settings["UICulture"] = item.Culture.Name;
                settings.Save();
            }
            catch { }
        }
    }

    private class ComboBoxItem
    {
        public CultureInfo Culture { get; set; }
        public string DisplayName { get; set; }
        public override string ToString() => DisplayName;
    }

    private void SetupTooltips()
    {
        toolTip.SetToolTip(btnClean, Properties.Resources.ToolTip_Clean);

        toolTip.SetToolTip(btnRefresh, Properties.Resources.ToolTip_Refresh);

        toolTip.SetToolTip(chkAutoClean, Properties.Resources.ToolTip_AutoClean);

        toolTip.SetToolTip(chkStartup, Properties.Resources.ToolTip_Startup);

        toolTip.SetToolTip(numInterval, Properties.Resources.ToolTip_Interval);

        toolTip.SetToolTip(listViewProcesses, Properties.Resources.ToolTip_ProcessesList);
    }

    private async System.Threading.Tasks.Task LoadProcessesAsync(System.Threading.CancellationToken ct = default)
    {
        try
        {
            SetLoadingState(true);

            listViewProcesses.BeginUpdate();
            listViewProcesses.Items.Clear();

            long thresholdMB = (long)numRamThreshold.Value;
            long thresholdBytes = thresholdMB * 1024 * 1024;
            var highUsageApps = await _processService.GetHighUsageProcessesAsync(thresholdBytes, ct);

            long totalMemory = 0;

            foreach (var app in highUsageApps)
            {
                ListViewItem item = new ListViewItem(app.Name);
                item.SubItems.Add(app.MemoryUsageDisplay);
                item.Tag = app;
                item.Checked = true;

                listViewProcesses.Items.Add(item);
                totalMemory += app.MemoryUsageBytes;
            }

            lblTotalProcesses.Text = highUsageApps.Count.ToString();
            lblTotalMemory.Text = FormatBytes(totalMemory);
            lblProcessCount.Text = string.Format(Properties.Resources.ProcessCountFormat, highUsageApps.Count);

            lblStatus.Text = string.Format(Properties.Resources.Status_LastUpdated, DateTime.Now.ToString("HH:mm:ss"));

            listViewProcesses.EndUpdate();

            SetupListViewColumns();
        }
        catch (OperationCanceledException)
        {
            _logger?.LogInformation("LoadProcessesAsync canceled");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to load processes");
            string errorMsg = $"{Properties.Resources.GenericErrorMessage}: {ex.Message}";

            lblStatus.Text = errorMsg;

            MessageBox.Show(errorMsg, Properties.Resources.ErrorTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void SetLoadingState(bool isLoading)
    {
        btnRefresh.Enabled = !isLoading;
        btnClean.Enabled = !isLoading;
    }

    private static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }

    private void ChkAutoClean_CheckedChanged(object sender, EventArgs e)
    {
        _timer.Stop();
        _timer.Tick -= Timer_Tick;

        if (chkAutoClean.Checked)
        {
            int intervalMinutes = (int)numInterval.Value;
            _timer.Interval = intervalMinutes * 60000;
            _timer.Tick += Timer_Tick;
            _timer.Start();

            lblStatus.Text = string.Format(Properties.Resources.Status_AutoCleanEnabled, intervalMinutes);
        }
        else
        {
            lblStatus.Text = Properties.Resources.Status_AutoCleanDisabled;
        }

        numInterval.Enabled = chkAutoClean.Checked;
    }

    private async void Timer_Tick(object? sender, EventArgs e)
    {
        if (!chkAutoClean.Checked)
            return;

        // prevent overlapping ticks
        if (System.Threading.Interlocked.Exchange(ref _isTickRunning, 1) == 1)
            return;

        try
        {
            long thresholdMB = (long)numRamThreshold.Value;
            long thresholdBytes = thresholdMB * 1024 * 1024;
            var highUsageApps = await _processService.GetHighUsageProcessesAsync(thresholdBytes);

            if (highUsageApps.Count > 0)
            {
                var processNames = highUsageApps.Select(p => p.Name).ToList();
                await _ramCleanerService.CleanMemoryAsync(processNames);

                lblStatus.Text = string.Format(Properties.Resources.Status_AutoCleaned, highUsageApps.Count);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Auto-clean failed");
        }
        finally
        {
            System.Threading.Interlocked.Exchange(ref _isTickRunning, 0);
        }
    }

    private async void NumRamThreshold_ValueChanged(object sender, EventArgs e)
    {
        UpdateRamThresholdInfo();
        await LoadProcessesAsync();
    }

    private void UpdateRamThresholdInfo()
    {
        long thresholdMB = (long)numRamThreshold.Value;
        lblRamThresholdInfo.Text = $"{thresholdMB} MB";
    }

    private void ChkStartup_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkStartup.Checked)
            {
                _startupService.EnableStartup();
                lblStatus.Text = Properties.Resources.StartupEnabled;
            }
            else
            {
                _startupService.DisableStartup();
                lblStatus.Text = Properties.Resources.StartupDisabled;
            }
        }
        catch (Exception ex)
        {
            chkStartup.Checked = !chkStartup.Checked;
            string errorMsg = string.Format(Properties.Resources.StartupChangeFailedFormat, ex.Message);
            MessageBox.Show(errorMsg, Properties.Resources.ErrorTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void NumInterval_ValueChanged(object sender, EventArgs e)
    {
        UpdateIntervalInfo();

        if (chkAutoClean.Checked)
        {
            _timer.Stop();
            int intervalMinutes = (int)numInterval.Value;
            _timer.Interval = intervalMinutes * 60000;
            _timer.Start();

            lblStatus.Text = string.Format(Properties.Resources.Status_AutoCleanEnabled, intervalMinutes);
        }
    }

    private void UpdateIntervalInfo()
    {
        int minutes = (int)numInterval.Value;
        lblIntervalInfo.Text = minutes == 1 ?
            string.Format(Properties.Resources.LabelInterval + " {0}", minutes) :
            string.Format(Properties.Resources.LabelInterval + " {0}", minutes);
    }

    private async void BtnClean_Click(object sender, EventArgs e)
    {
        var selectedProcesses = listViewProcesses.CheckedItems
            .Cast<ListViewItem>()
            .Select(i => i.Text)
            .ToList();

        if (selectedProcesses.Count == 0)
        {
            MessageBox.Show(Properties.Resources.Msg_SelectApplications, Properties.Resources.ErrorTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        try
        {
            SetLoadingState(true);

            lblStatus.Text = Properties.Resources.Status_Cleaning;

            await _ramCleanerService.CleanMemoryAsync(selectedProcesses);

            lblStatus.Text = string.Format(Properties.Resources.Msg_CleanedApplications, selectedProcesses.Count);

            await System.Threading.Tasks.Task.Delay(500);
            await LoadProcessesAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Clean failed");
            string errorMsg = string.Format(Properties.Resources.StartupChangeFailedFormat, ex.Message);
            lblStatus.Text = errorMsg;
            MessageBox.Show(errorMsg, Properties.Resources.ErrorTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        _ = LoadProcessesAsync();
    }

    private void BtnChangeLanguage_Click(object? sender, EventArgs e)
    {
        // legacy handler left for compatibility - prefer combobox
        if (_cmbLanguage != null && _cmbLanguage.SelectedItem is ComboBoxItem item)
        {
            var newCulture = item.Culture;
            _localizationService.SetCulture(newCulture);
            _localizationService.ApplyResources(this);
            try
            {
                var settings = RamCleaner.WinForms.Properties.Settings.Default;
                settings["UICulture"] = newCulture.Name;
                settings.Save();
            }
            catch { }
        }
    }

    private void BtnSelectAll_Click(object sender, EventArgs e)
    {
        listViewProcesses.BeginUpdate();

        foreach (ListViewItem item in listViewProcesses.Items)
        {
            item.Checked = true;
        }

        listViewProcesses.EndUpdate();
    }

    private void BtnSelectNone_Click(object sender, EventArgs e)
    {
        listViewProcesses.BeginUpdate();

        foreach (ListViewItem item in listViewProcesses.Items)
        {
            item.Checked = false;
        }

        listViewProcesses.EndUpdate();
    }

    private void LnkYoutube_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        string youtubeUrl = "https://www.youtube.com/watch?v=WOCq0QpkdW8&list=RDWOCq0QpkdW8&start_radio=1";

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = youtubeUrl,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            string errorMsg = string.Format(Properties.Resources.LinkOpenFailed, ex.Message);
            MessageBox.Show(errorMsg, Properties.Resources.ErrorTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!_isClosing)
        {
            e.Cancel = true;
            this.Hide();
            notifyIcon.Visible = true;

            string message = Properties.Resources.TrayMinimizedMessage;
            notifyIcon.BalloonTipTitle = Properties.Resources.TrayTitle;
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(2000);
        }
    }

    private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;
        this.Activate();
        notifyIcon.Visible = false;
    }

    private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;
        this.Activate();
        notifyIcon.Visible = false;
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _isClosing = true;
        notifyIcon.Visible = false;
        Application.Exit();
    }
}
