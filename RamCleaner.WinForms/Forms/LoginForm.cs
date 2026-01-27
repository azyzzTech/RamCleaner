using RamCleaner.WinForms.Core.Services;
using System.Globalization;
using System.Diagnostics;

namespace RamCleaner.WinForms.Forms;

internal partial class LoginForm : Form
{
    private readonly IAuthService _authService;
    private bool _isTurkish = CultureInfo.CurrentUICulture.Name.StartsWith("tr");

    public LoginForm(IAuthService authService)
    {
        _authService = authService;

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
            bool isAuthorized = await _authService.FullAuthFlowAsync();

            if (isAuthorized)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(_isTurkish ?
                    "Giris basarisiz veya yetkiniz yok!" :
                    "Login failed or insufficient permissions!",
                    "SupremeLegends Auth", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
                lblStatus.Text = "Hata!";
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
