namespace RamCleaner.WinForms.Forms;

partial class LoginForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
        btnLogin = new Button();
        lblStatus = new Label();
        SuspendLayout();
        // 
        // btnLogin
        // 
        btnLogin.AccessibleRole = AccessibleRole.None;
        btnLogin.BackColor = Color.Transparent;
        btnLogin.BackgroundImage = (Image)resources.GetObject("btnLogin.BackgroundImage");
        btnLogin.BackgroundImageLayout = ImageLayout.Zoom;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.FlatAppearance.MouseDownBackColor = Color.Transparent;
        btnLogin.FlatAppearance.MouseOverBackColor = Color.Transparent;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.Location = new Point(284, 73);
        btnLogin.Name = "btnLogin";
        btnLogin.RightToLeft = RightToLeft.No;
        btnLogin.Size = new Size(233, 166);
        btnLogin.TabIndex = 0;
        btnLogin.UseVisualStyleBackColor = false;
        btnLogin.Click += btnLogin_Click;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStatus.ForeColor = Color.White;
        lblStatus.Location = new Point(284, 274);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(0, 25);
        lblStatus.TabIndex = 1;
        // 
        // LoginForm
        // 
        AutoScaleDimensions = new SizeF(8F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(128, 128, 255);
        ClientSize = new Size(800, 450);
        Controls.Add(lblStatus);
        Controls.Add(btnLogin);
        Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "RAM Cleaner";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnLogin;
    private Label lblStatus;
}