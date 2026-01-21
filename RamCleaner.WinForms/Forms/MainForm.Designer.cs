namespace RamCleaner.WinForms.Forms;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            if (notifyIcon != null)
            {
                notifyIcon.Dispose();
            }
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        listViewProcesses = new ListView();
        ApplicationName = new ColumnHeader();
        Memory = new ColumnHeader();
        btnClean = new Button();
        numInterval = new NumericUpDown();
        chkAutoClean = new CheckBox();
        lblStatus = new Label();
        btnRefresh = new Button();
        lnkYoutube = new LinkLabel();
        panelHeader = new Panel();
        lblTitle = new Label();
        panelStats = new Panel();
        lblTotalProcesses = new Label();
        lblTotalMemory = new Label();
        label3 = new Label();
        label2 = new Label();
        groupBoxActions = new GroupBox();
        lblRamThresholdInfo = new Label();
        label4 = new Label();
        numRamThreshold = new NumericUpDown();
        chkStartup = new CheckBox();
        lblIntervalInfo = new Label();
        label1 = new Label();
        groupBoxProcesses = new GroupBox();
        lblProcessCount = new Label();
        btnSelectAll = new Button();
        btnSelectNone = new Button();
        panelFooter = new Panel();
        notifyIcon = new NotifyIcon(components);
        contextMenuStrip = new ContextMenuStrip(components);
        showToolStripMenuItem = new ToolStripMenuItem();
        exitToolStripMenuItem = new ToolStripMenuItem();
        toolTip = new ToolTip(components);
        ((System.ComponentModel.ISupportInitialize)numInterval).BeginInit();
        panelHeader.SuspendLayout();
        panelStats.SuspendLayout();
        groupBoxActions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numRamThreshold).BeginInit();
        groupBoxProcesses.SuspendLayout();
        panelFooter.SuspendLayout();
        contextMenuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // listViewProcesses
        // 
        listViewProcesses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        listViewProcesses.BackColor = Color.FromArgb(20, 20, 20);
        listViewProcesses.BorderStyle = BorderStyle.FixedSingle;
        listViewProcesses.CheckBoxes = true;
        listViewProcesses.Columns.AddRange(new ColumnHeader[] { ApplicationName, Memory });
        listViewProcesses.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        listViewProcesses.ForeColor = Color.White;
        listViewProcesses.FullRowSelect = true;
        listViewProcesses.GridLines = true;
        listViewProcesses.Location = new Point(12, 52);
        listViewProcesses.Name = "listViewProcesses";
        listViewProcesses.Size = new Size(560, 335);
        listViewProcesses.TabIndex = 0;
        listViewProcesses.UseCompatibleStateImageBehavior = false;
        listViewProcesses.View = View.Details;
        // 
        // ApplicationName
        // 
        ApplicationName.Text = "Application Name";
        ApplicationName.Width = 380;
        // 
        // Memory
        // 
        Memory.Text = "Memory Usage";
        Memory.TextAlign = HorizontalAlignment.Right;
        Memory.Width = 180;
        // 
        // btnClean
        // 
        btnClean.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClean.BackColor = Color.Maroon;
        btnClean.FlatAppearance.BorderColor = Color.Red;
        btnClean.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 92, 154);
        btnClean.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 102, 179);
        btnClean.FlatStyle = FlatStyle.Flat;
        btnClean.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnClean.ForeColor = Color.White;
        btnClean.Location = new Point(594, 66);
        btnClean.Name = "btnClean";
        btnClean.Size = new Size(184, 36);
        btnClean.TabIndex = 1;
        btnClean.Text = "Clean Now";
        btnClean.UseVisualStyleBackColor = false;
        btnClean.Click += BtnClean_Click;
        // 
        // numInterval
        // 
        numInterval.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        numInterval.BackColor = Color.FromArgb(25, 25, 25);
        numInterval.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        numInterval.ForeColor = Color.White;
        numInterval.Location = new Point(13, 74);
        numInterval.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
        numInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numInterval.Name = "numInterval";
        numInterval.Size = new Size(100, 25);
        numInterval.TabIndex = 2;
        numInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
        numInterval.ValueChanged += NumInterval_ValueChanged;
        // 
        // chkAutoClean
        // 
        chkAutoClean.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        chkAutoClean.AutoSize = true;
        chkAutoClean.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        chkAutoClean.ForeColor = Color.White;
        chkAutoClean.Location = new Point(45, 24);
        chkAutoClean.Name = "chkAutoClean";
        chkAutoClean.Size = new Size(101, 23);
        chkAutoClean.TabIndex = 3;
        chkAutoClean.Text = "Auto Clean";
        chkAutoClean.TextAlign = ContentAlignment.MiddleCenter;
        chkAutoClean.UseVisualStyleBackColor = false;
        chkAutoClean.CheckedChanged += ChkAutoClean_CheckedChanged;
        // 
        // lblStatus
        // 
        lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblStatus.ForeColor = Color.White;
        lblStatus.Location = new Point(12, 9);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(51, 19);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "Ready";
        // 
        // btnRefresh
        // 
        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.BackColor = Color.FromArgb(25, 25, 25);
        btnRefresh.FlatAppearance.BorderColor = Color.Red;
        btnRefresh.FlatAppearance.MouseDownBackColor = Color.FromArgb(35, 35, 35);
        btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(594, 108);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(184, 40);
        btnRefresh.TabIndex = 5;
        btnRefresh.Text = "Refresh List";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += BtnRefresh_Click;
        // 
        // lnkYoutube
        // 
        lnkYoutube.ActiveLinkColor = Color.Red;
        lnkYoutube.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lnkYoutube.AutoSize = true;
        lnkYoutube.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lnkYoutube.LinkColor = Color.Red;
        lnkYoutube.Location = new Point(627, 13);
        lnkYoutube.Name = "lnkYoutube";
        lnkYoutube.Size = new Size(113, 15);
        lnkYoutube.TabIndex = 6;
        lnkYoutube.TabStop = true;
        lnkYoutube.Text = "Follow Us: YouTube";
        lnkYoutube.VisitedLinkColor = Color.Red;
        lnkYoutube.LinkClicked += LnkYoutube_LinkClicked;
        // 
        // panelHeader
        // 
        panelHeader.BackColor = Color.FromArgb(15, 15, 15);
        panelHeader.Controls.Add(lblTitle);
        panelHeader.Dock = DockStyle.Top;
        panelHeader.Location = new Point(0, 0);
        panelHeader.Name = "panelHeader";
        panelHeader.Size = new Size(800, 60);
        panelHeader.TabIndex = 7;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(15, 13);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(163, 32);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "RAM Cleaner";
        // 
        // panelStats
        // 
        panelStats.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        panelStats.BackColor = Color.FromArgb(25, 25, 25);
        panelStats.BorderStyle = BorderStyle.FixedSingle;
        panelStats.Controls.Add(lblTotalProcesses);
        panelStats.Controls.Add(lblTotalMemory);
        panelStats.Controls.Add(label3);
        panelStats.Controls.Add(label2);
        panelStats.Location = new Point(594, 154);
        panelStats.Name = "panelStats";
        panelStats.Size = new Size(184, 111);
        panelStats.TabIndex = 8;
        // 
        // lblTotalProcesses
        // 
        lblTotalProcesses.AutoSize = true;
        lblTotalProcesses.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTotalProcesses.ForeColor = Color.Maroon;
        lblTotalProcesses.Location = new Point(12, 27);
        lblTotalProcesses.Name = "lblTotalProcesses";
        lblTotalProcesses.Size = new Size(24, 28);
        lblTotalProcesses.TabIndex = 3;
        lblTotalProcesses.Text = "0";
        // 
        // lblTotalMemory
        // 
        lblTotalMemory.AutoSize = true;
        lblTotalMemory.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTotalMemory.ForeColor = Color.Maroon;
        lblTotalMemory.Location = new Point(11, 74);
        lblTotalMemory.Name = "lblTotalMemory";
        lblTotalMemory.Size = new Size(62, 28);
        lblTotalMemory.TabIndex = 2;
        lblTotalMemory.Text = "0 MB";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label3.ForeColor = Color.White;
        label3.Location = new Point(11, 55);
        label3.Name = "label3";
        label3.Size = new Size(108, 19);
        label3.TabIndex = 1;
        label3.Text = "Total Memory:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label2.ForeColor = Color.White;
        label2.Location = new Point(12, 8);
        label2.Name = "label2";
        label2.Size = new Size(123, 19);
        label2.TabIndex = 0;
        label2.Text = "Processes Found:";
        // 
        // groupBoxActions
        // 
        groupBoxActions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        groupBoxActions.BackColor = Color.FromArgb(20, 20, 20);
        groupBoxActions.Controls.Add(lblRamThresholdInfo);
        groupBoxActions.Controls.Add(label4);
        groupBoxActions.Controls.Add(numRamThreshold);
        groupBoxActions.Controls.Add(chkStartup);
        groupBoxActions.Controls.Add(lblIntervalInfo);
        groupBoxActions.Controls.Add(label1);
        groupBoxActions.Controls.Add(numInterval);
        groupBoxActions.Controls.Add(chkAutoClean);
        groupBoxActions.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        groupBoxActions.ForeColor = Color.White;
        groupBoxActions.Location = new Point(594, 280);
        groupBoxActions.Name = "groupBoxActions";
        groupBoxActions.Size = new Size(184, 201);
        groupBoxActions.TabIndex = 9;
        groupBoxActions.TabStop = false;
        groupBoxActions.Text = "Auto Clean Settings";
        // 
        // lblRamThresholdInfo
        // 
        lblRamThresholdInfo.Font = new Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Italic);
        lblRamThresholdInfo.ForeColor = Color.FromArgb(150, 150, 150);
        lblRamThresholdInfo.Location = new Point(124, 105);
        lblRamThresholdInfo.Name = "lblRamThresholdInfo";
        lblRamThresholdInfo.Size = new Size(54, 18);
        lblRamThresholdInfo.TabIndex = 8;
        lblRamThresholdInfo.Text = "500 MB";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label4.ForeColor = Color.White;
        label4.Location = new Point(12, 102);
        label4.Name = "label4";
        label4.Size = new Size(115, 19);
        label4.TabIndex = 7;
        label4.Text = "RAM Threshold:";
        // 
        // numRamThreshold
        // 
        numRamThreshold.BackColor = Color.FromArgb(25, 25, 25);
        numRamThreshold.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        numRamThreshold.ForeColor = Color.White;
        numRamThreshold.Location = new Point(13, 124);
        numRamThreshold.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numRamThreshold.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
        numRamThreshold.Name = "numRamThreshold";
        numRamThreshold.Size = new Size(100, 25);
        numRamThreshold.TabIndex = 6;
        numRamThreshold.Value = new decimal(new int[] { 500, 0, 0, 0 });
        numRamThreshold.ValueChanged += NumRamThreshold_ValueChanged;
        // 
        // chkStartup
        // 
        chkStartup.AutoSize = true;
        chkStartup.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        chkStartup.ForeColor = Color.White;
        chkStartup.Location = new Point(13, 163);
        chkStartup.Name = "chkStartup";
        chkStartup.Size = new Size(157, 23);
        chkStartup.TabIndex = 5;
        chkStartup.Text = "Start with Windows";
        chkStartup.UseVisualStyleBackColor = false;
        chkStartup.CheckedChanged += ChkStartup_CheckedChanged;
        // 
        // lblIntervalInfo
        // 
        lblIntervalInfo.Font = new Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Italic);
        lblIntervalInfo.ForeColor = Color.FromArgb(150, 150, 150);
        lblIntervalInfo.Location = new Point(83, 53);
        lblIntervalInfo.Name = "lblIntervalInfo";
        lblIntervalInfo.Size = new Size(86, 18);
        lblIntervalInfo.TabIndex = 4;
        lblIntervalInfo.Text = "5 minutes";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label1.ForeColor = Color.White;
        label1.Location = new Point(12, 50);
        label1.Name = "label1";
        label1.Size = new Size(65, 19);
        label1.TabIndex = 3;
        label1.Text = "Interval:";
        // 
        // groupBoxProcesses
        // 
        groupBoxProcesses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBoxProcesses.BackColor = Color.FromArgb(20, 20, 20);
        groupBoxProcesses.Controls.Add(lblProcessCount);
        groupBoxProcesses.Controls.Add(btnSelectAll);
        groupBoxProcesses.Controls.Add(btnSelectNone);
        groupBoxProcesses.Controls.Add(listViewProcesses);
        groupBoxProcesses.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        groupBoxProcesses.ForeColor = Color.White;
        groupBoxProcesses.Location = new Point(12, 66);
        groupBoxProcesses.Name = "groupBoxProcesses";
        groupBoxProcesses.Size = new Size(576, 400);
        groupBoxProcesses.TabIndex = 10;
        groupBoxProcesses.TabStop = false;
        groupBoxProcesses.Text = "High Memory Usage Processes";
        // 
        // lblProcessCount
        // 
        lblProcessCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblProcessCount.AutoSize = true;
        lblProcessCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblProcessCount.ForeColor = Color.FromArgb(150, 150, 150);
        lblProcessCount.Location = new Point(12, 377);
        lblProcessCount.Name = "lblProcessCount";
        lblProcessCount.Size = new Size(71, 15);
        lblProcessCount.TabIndex = 3;
        lblProcessCount.Text = "0 processes";
        // 
        // btnSelectAll
        // 
        btnSelectAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSelectAll.BackColor = Color.FromArgb(25, 25, 25);
        btnSelectAll.FlatAppearance.BorderColor = Color.Red;
        btnSelectAll.FlatAppearance.MouseDownBackColor = Color.FromArgb(35, 35, 35);
        btnSelectAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
        btnSelectAll.FlatStyle = FlatStyle.Flat;
        btnSelectAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSelectAll.ForeColor = Color.White;
        btnSelectAll.Location = new Point(389, 370);
        btnSelectAll.Name = "btnSelectAll";
        btnSelectAll.Size = new Size(85, 32);
        btnSelectAll.TabIndex = 2;
        btnSelectAll.Text = "Select All";
        btnSelectAll.UseVisualStyleBackColor = false;
        btnSelectAll.Click += BtnSelectAll_Click;
        // 
        // btnSelectNone
        // 
        btnSelectNone.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSelectNone.BackColor = Color.FromArgb(25, 25, 25);
        btnSelectNone.FlatAppearance.BorderColor = Color.Red;
        btnSelectNone.FlatAppearance.MouseDownBackColor = Color.FromArgb(35, 35, 35);
        btnSelectNone.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
        btnSelectNone.FlatStyle = FlatStyle.Flat;
        btnSelectNone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSelectNone.ForeColor = Color.White;
        btnSelectNone.Location = new Point(480, 370);
        btnSelectNone.Name = "btnSelectNone";
        btnSelectNone.Size = new Size(90, 32);
        btnSelectNone.TabIndex = 1;
        btnSelectNone.Text = "Select None";
        btnSelectNone.UseVisualStyleBackColor = false;
        btnSelectNone.Click += BtnSelectNone_Click;
        // 
        // panelFooter
        // 
        panelFooter.BackColor = Color.FromArgb(15, 15, 15);
        panelFooter.Controls.Add(lblStatus);
        panelFooter.Controls.Add(lnkYoutube);
        panelFooter.Dock = DockStyle.Bottom;
        panelFooter.Location = new Point(0, 487);
        panelFooter.Name = "panelFooter";
        panelFooter.Size = new Size(800, 49);
        panelFooter.TabIndex = 11;
        // 
        // notifyIcon
        // 
        notifyIcon.ContextMenuStrip = contextMenuStrip;
        notifyIcon.Text = "SupremeLegends RAM Cleaner";
        notifyIcon.Visible = true;
        notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        // 
        // contextMenuStrip
        // 
        contextMenuStrip.ImageScalingSize = new Size(20, 20);
        contextMenuStrip.Items.AddRange(new ToolStripItem[] { showToolStripMenuItem, exitToolStripMenuItem });
        contextMenuStrip.Name = "contextMenuStrip";
        contextMenuStrip.Size = new Size(104, 48);
        // 
        // showToolStripMenuItem
        // 
        showToolStripMenuItem.Name = "showToolStripMenuItem";
        showToolStripMenuItem.Size = new Size(103, 22);
        showToolStripMenuItem.Text = "Show";
        showToolStripMenuItem.Click += ShowToolStripMenuItem_Click;
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(103, 22);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(8F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(18, 18, 18);
        ClientSize = new Size(800, 536);
        Controls.Add(panelFooter);
        Controls.Add(groupBoxProcesses);
        Controls.Add(panelStats);
        Controls.Add(panelHeader);
        Controls.Add(btnRefresh);
        Controls.Add(btnClean);
        Controls.Add(groupBoxActions);
        Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "RAM Cleaner";
        FormClosing += MainForm_FormClosing;
        ((System.ComponentModel.ISupportInitialize)numInterval).EndInit();
        panelHeader.ResumeLayout(false);
        panelHeader.PerformLayout();
        panelStats.ResumeLayout(false);
        panelStats.PerformLayout();
        groupBoxActions.ResumeLayout(false);
        groupBoxActions.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numRamThreshold).EndInit();
        groupBoxProcesses.ResumeLayout(false);
        groupBoxProcesses.PerformLayout();
        panelFooter.ResumeLayout(false);
        panelFooter.PerformLayout();
        contextMenuStrip.ResumeLayout(false);
        ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listViewProcesses;
    private System.Windows.Forms.ColumnHeader ApplicationName;
    private System.Windows.Forms.ColumnHeader Memory;
    private System.Windows.Forms.Button btnClean;
    private System.Windows.Forms.NumericUpDown numInterval;
    private System.Windows.Forms.CheckBox chkAutoClean;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.LinkLabel lnkYoutube;
    private System.Windows.Forms.Panel panelHeader;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Panel panelStats;
    private System.Windows.Forms.Label lblTotalProcesses;
    private System.Windows.Forms.Label lblTotalMemory;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.GroupBox groupBoxActions;
    private System.Windows.Forms.CheckBox chkStartup;
    private System.Windows.Forms.Label lblIntervalInfo;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBoxProcesses;
    private System.Windows.Forms.Label lblProcessCount;
    private System.Windows.Forms.Button btnSelectAll;
    private System.Windows.Forms.Button btnSelectNone;
    private System.Windows.Forms.Panel panelFooter;
    private System.Windows.Forms.NotifyIcon notifyIcon;
    private System.Windows.Forms.NumericUpDown numRamThreshold;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label lblRamThresholdInfo;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolTip toolTip;
}