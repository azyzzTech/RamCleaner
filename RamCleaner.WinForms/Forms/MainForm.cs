using Microsoft.VisualBasic.ApplicationServices;
using RamCleaner.WinForms.Business;
using RamCleaner.WinForms.Services;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RamCleaner.WinForms.Forms;

internal partial class MainForm : Form
{
    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

    private System.Windows.Forms.Timer _timer = new();
    private System.Windows.Forms.Timer _refreshTimer = new();
    private bool _isClosing = false;
    private bool _isTurkish = CultureInfo.CurrentUICulture.Name.StartsWith("tr");
    private RamBusiness _ramBusiness = new();

    internal MainForm()
    {
        InitializeComponent();

        ApplyDarkThemeToListView();
        SetupListViewColumns();
        SetupLanguage();
        SetupTooltips();
        LoadStartupState();
        LoadProcesses();

        _refreshTimer.Interval = 60000;
        _refreshTimer.Tick += (s, e) =>
        {
            if (!chkAutoClean.Checked)
            {
                LoadProcesses();
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
            chkStartup.Checked = StartupService.IsStartupEnabled();
        }
        catch
        {
            chkStartup.Checked = false;
        }
    }

    private void SetupLanguage()
    {
        this.Text = _isTurkish ? "SupremeLegends RAM Temizleyici" : "SupremeLegends RAM Cleaner";
        ApplicationName.Text = _isTurkish ? "Uygulama Adı" : "Application Name";
        btnClean.Text = _isTurkish ? "Şimdi Temizle" : "Clean Now";
        btnRefresh.Text = _isTurkish ? "Listeyi Yenile" : "Refresh List";
        btnSelectAll.Text = _isTurkish ? "Tümünü Seç" : "Select All";
        btnSelectNone.Text = _isTurkish ? "Hiçbirini Seçme" : "Select None";
        chkAutoClean.Text = _isTurkish ? "Otomatik Temizle" : "Auto Clean";
        chkStartup.Text = _isTurkish ? "Windows ile Başlat" : "Start with Windows";
        lblTitle.Text = _isTurkish ? "SupremeLegends RAM Temizleyici" : "SupremeLegends RAM Cleaner";
        Memory.Text = _isTurkish ? "Bellek Kullanımı" : "Memory Usage";
        notifyIcon.Text = _isTurkish ? "SupremeLegends RAM Temizleyici" : "SupremeLegends RAM Cleaner";
        lnkYoutube.Text = _isTurkish ? "Bizi Takip Edin: YouTube" : "Follow Us: YouTube";
        groupBoxProcesses.Text = _isTurkish ? "Yüksek Bellek Kullanan Süreçler" : "High Memory Usage Processes";
        showToolStripMenuItem.Text = _isTurkish ? "Göster" : "Show";
        groupBoxActions.Text = _isTurkish ? "Otomatik Temizleme Ayarları" : "Auto Clean Settings";
        exitToolStripMenuItem.Text = _isTurkish ? "Çıkış" : "Exit";
        label1.Text = _isTurkish ? "Aralık:" : "Interval:";
        label2.Text = _isTurkish ? "Bulunan Süreçler:" : "Processes Found:";
        label3.Text = _isTurkish ? "Toplam Bellek:" : "Total Memory:";
        label4.Text = _isTurkish ? "RAM Eşiği:" : "RAM Threshold:";

        UpdateRamThresholdInfo();
        UpdateIntervalInfo();
    }

    private void SetupTooltips()
    {
        toolTip.SetToolTip(btnClean, _isTurkish ?
            "Seçili uygulamaların bellek kullanımını optimize eder" :
            "Optimizes memory usage for selected applications");

        toolTip.SetToolTip(btnRefresh, _isTurkish ?
            "Uygulama listesini yeniler" :
            "Refreshes the application list");

        toolTip.SetToolTip(chkAutoClean, _isTurkish ?
            "Belirlenen aralıklarla otomatik olarak bellek temizleme yapar" :
            "Automatically cleans memory at specified intervals");

        toolTip.SetToolTip(chkStartup, _isTurkish ?
            "Uygulamanın Windows başladığında otomatik olarak çalışmasını sağlar" :
            "Automatically start the application when Windows starts");

        toolTip.SetToolTip(numInterval, _isTurkish ?
            "Otomatik temizleme aralığı (dakika)" :
            "Auto clean interval (minutes)");

        toolTip.SetToolTip(listViewProcesses, _isTurkish ?
            "Temizlemek için uygulamaları seçin/kaldırın" :
            "Check/uncheck applications to clean");
    }

    private void LoadProcesses()
    {
        try
        {
            SetLoadingState(false);

            listViewProcesses.BeginUpdate();
            listViewProcesses.Items.Clear();

            ProcessBusiness processBusiness = new();
            long thresholdMB = (long)numRamThreshold.Value;
            long thresholdBytes = thresholdMB * 1024 * 1024;
            var highUsageApps = processBusiness.GetHighUsageProcesses(thresholdBytes);

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
            lblProcessCount.Text = _isTurkish ?
                $"{highUsageApps.Count} süreç" :
                $"{highUsageApps.Count} processes";

            lblStatus.Text = _isTurkish ?
                $"Son güncelleme: {DateTime.Now:HH:mm:ss}" :
                $"Last updated: {DateTime.Now:HH:mm:ss}";

            listViewProcesses.EndUpdate();

            SetupListViewColumns();
        }
        catch (Exception ex)
        {
            string errorMsg = _isTurkish ?
                $"Hata oluştu: {ex.Message}" :
                $"An error occurred: {ex.Message}";

            lblStatus.Text = errorMsg;

            MessageBox.Show(errorMsg, _isTurkish ? "Hata" : "Error",
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

            lblStatus.Text = _isTurkish ?
                $"Otomatik temizleme aktif: Her {intervalMinutes} dakikada bir" :
                $"Auto clean active: Every {intervalMinutes} minute(s)";
        }
        else
        {
            lblStatus.Text = _isTurkish ?
                "Otomatik temizleme devre dışı" :
                "Auto clean disabled";
        }

        numInterval.Enabled = chkAutoClean.Checked;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (chkAutoClean.Checked)
        {
            ProcessBusiness processBusiness = new();
            long thresholdMB = (long)numRamThreshold.Value;
            long thresholdBytes = thresholdMB * 1024 * 1024;
            var highUsageApps = processBusiness.GetHighUsageProcesses(thresholdBytes);

            if (highUsageApps.Count > 0)
            {
                var processNames = highUsageApps.Select(p => p.Name).ToList();
                _ramBusiness.CleanMemory(processNames);

                lblStatus.Text = _isTurkish ?
                    $"Otomatik temizlendi: {highUsageApps.Count} uygulama" :
                    $"Auto cleaned: {highUsageApps.Count} application(s)";
            }
        }
    }

    private void NumRamThreshold_ValueChanged(object sender, EventArgs e)
    {
        UpdateRamThresholdInfo();
        LoadProcesses();
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
                StartupService.EnableStartup();
                lblStatus.Text = _isTurkish ?
                    "Windows ile başlatma etkinleştirildi" :
                    "Start with Windows enabled";
            }
            else
            {
                StartupService.DisableStartup();
                lblStatus.Text = _isTurkish ?
                    "Windows ile başlatma devre dışı bırakıldı" :
                    "Start with Windows disabled";
            }
        }
        catch (Exception ex)
        {
            chkStartup.Checked = !chkStartup.Checked;
            string errorMsg = _isTurkish ?
                $"Başlatma ayarı değiştirilemedi: {ex.Message}" :
                $"Failed to change startup setting: {ex.Message}";

            MessageBox.Show(errorMsg, _isTurkish ? "Hata" : "Error",
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

            lblStatus.Text = _isTurkish ?
                $"Otomatik temizleme aktif: Her {intervalMinutes} dakikada bir" :
                $"Auto clean active: Every {intervalMinutes} minute(s)";
        }
    }

    private void UpdateIntervalInfo()
    {
        int minutes = (int)numInterval.Value;
        lblIntervalInfo.Text = _isTurkish ?
            $"{minutes} dakika" :
            $"{minutes} minute{(minutes != 1 ? "s" : "")}";
    }

    private void BtnClean_Click(object sender, EventArgs e)
    {
        var selectedProcesses = listViewProcesses.CheckedItems
            .Cast<ListViewItem>()
            .Select(i => i.Text)
            .ToList();

        if (selectedProcesses.Count == 0)
        {
            string msg = _isTurkish ?
                "Lütfen temizlenecek uygulamaları seçin." :
                "Please select applications to clean.";

            MessageBox.Show(msg, _isTurkish ? "Uyarı" : "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        try
        {
            SetLoadingState(true);

            lblStatus.Text = _isTurkish ?
                "Bellek temizleniyor..." :
                "Cleaning memory...";

            Application.DoEvents();

            _ramBusiness.CleanMemory(selectedProcesses);

            lblStatus.Text = _isTurkish ?
                $"Bellek optimize edildi! {selectedProcesses.Count} uygulama temizlendi." :
                $"Memory optimized! Cleaned {selectedProcesses.Count} application(s).";

            Thread.Sleep(500);
            LoadProcesses();
        }
        catch (Exception ex)
        {
            string errorMsg = _isTurkish ?
                $"Temizleme hatası: {ex.Message}" :
                $"Clean error: {ex.Message}";
            lblStatus.Text = errorMsg;
            MessageBox.Show(errorMsg, _isTurkish ? "Hata" : "Error",
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
        LoadProcesses();
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
            string errorMsg = _isTurkish ?
                $"Link açılamadı: {ex.Message}" :
                $"Could not open link: {ex.Message}";
            MessageBox.Show(errorMsg, _isTurkish ? "Hata" : "Error",
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

            string message = _isTurkish ?
                "Uygulama sistem tepsisine küçültüldü. Tekrar açmak için çift tıklayın." :
                "Application minimized to system tray. Double-click to restore.";
            notifyIcon.BalloonTipTitle = _isTurkish ? "RAM Temizleyici" : "RAM Cleaner";
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
