using RamCleaner.WinForms.InfraStructure;
using System.Globalization;

namespace RamCleaner.WinForms.Forms;

internal partial class LoginForm : Form
{
    private bool _isTurkish = CultureInfo.CurrentUICulture.Name.StartsWith("tr");
    private readonly DiscordAuth _authManager = new DiscordAuth();

    internal LoginForm()
    {
        InitializeComponent();

        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
    }

    internal async void btnLogin_Click(object sender, EventArgs e)
    {
        btnLogin.Enabled = false;
        lblStatus.Text = _isTurkish ? "Discord Onayı Bekleniyor..." : "Waiting for Discord...";

        // Tüm akış (Tarayıcı açma + Token yakalama + Rol kontrolü) burada biter
        bool isAuthorized = await _authManager.FullAuthFlowAsync();

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
}
