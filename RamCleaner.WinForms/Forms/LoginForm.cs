using RamCleaner.WinForms.Core.Services;
using System.Globalization;
using System.Diagnostics;

namespace RamCleaner.WinForms.Forms;

/// <summary>
/// Login form that triggers Discord authentication and reports status to the user.
/// </summary>
internal partial class LoginForm : Form
{
    private readonly IAuthService _auth_service;
    private bool _isTurkish = CultureInfo.CurrentUICulture.Name.StartsWith("tr");

    public LoginForm(IAuthService authService)
    {
        _auth_service = authService;

        InitializeComponent();

        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        btnLogin.Enabled = false;
        lblStatus.Text = _isTurkish ? "Discord Onayı Bekleniyor..." : "Waiting for Discord...";

        try
        {
            bool isAuthorized = await _auth_service.FullAuthFlowAsync();

            if (isAuthorized)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(Properties.Resources.LoginFailedMessage,
                    Properties.Resources.AuthTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
                lblStatus.Text = Properties.Resources.GenericErrorMessage;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            MessageBox.Show(_isTurkish ? "Bilinmeyen bir hata oldu" : "An error occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnLogin.Enabled = true;
            lblStatus.Text = _isTurkish ? "Hata!" : "Error";
        }
    }
}
