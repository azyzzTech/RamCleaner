using Newtonsoft.Json;
using RamCleaner.WinForms.Core.Models;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;

namespace RamCleaner.WinForms.InfraStructure;

internal class DiscordAuth
{
    private bool _isTurkish = CultureInfo.CurrentUICulture.Name.StartsWith("tr");
    private const string ClientId = "1463542451460636828";
    private const string RedirectUri = "http://localhost:5000/";
    private const string ApiBaseUrl = "https://ram-cleaner-dc-auth-api.onrender.com";

    public async Task<bool> FullAuthFlowAsync()
    {
        DateTime lastAuth = Properties.Settings.Default.LastAuthDate;
        bool isStillValid = (DateTime.Now - lastAuth).TotalDays < 7;
        bool wasAuthorized = Properties.Settings.Default.IsAuthorized;

        if (wasAuthorized && isStillValid)
        {
            return true;
        }

        try
        {
            string authUrl = $"https://discord.com/api/oauth2/authorize?client_id={ClientId}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&response_type=token&scope=identify";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = authUrl,
                UseShellExecute = true
            };
            Process.Start(psi);

            string accessToken = await ListenForAccessToken();

            if (string.IsNullOrEmpty(accessToken))
                return false;

            using var client = new HttpClient();
            var response = await client.GetAsync($"{ApiBaseUrl}/check-auth?access_token={accessToken}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuthResponse>(jsonResponse);

                bool isAuthorized = result != null && result.Authorized;

                if (isAuthorized)
                {
                    Properties.Settings.Default.LastAuthDate = DateTime.Now;
                    Properties.Settings.Default.IsAuthorized = true;
                    Properties.Settings.Default.Save();
                }

                return isAuthorized;
            }

            return false;
        }
        catch { return false; }
    }

    private async Task<string> ListenForAccessToken()
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add(RedirectUri);
        listener.Start();

        string accessToken = null;

        while (accessToken == null)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;
            var response = context.Response;

            if (request.Url.AbsolutePath == "/")
            {
                string title = _isTurkish ? "Doğrulanıyor..." : "Authorizing...";
                string subText = _isTurkish ? "Lütfen bekleyiniz, yönlendiriliyorsunuz." : "Please wait, you are being redirected.";
                string successTitle = _isTurkish ? "Başarılı!" : "Success!";
                string successSub = _isTurkish ? "Giriş yapıldı. Bu sekmeyi kapatabilirsiniz." : "Login successful. You can close this tab.";

                string responseHtml = $@"
                <html>
                <body style='background:#23272a; color:white; font-family:sans-serif; text-align:center; padding-top:100px;'>
                    <h1>{title}</h1>
                    <p>{subText}</p>
                    <script>
                        const hash = window.location.hash;
                        if (hash) {{
                            const params = new URLSearchParams(hash.substring(1));
                            const token = params.get('access_token');
                            if (token) {{
                                fetch('/token?val=' + token);
                                document.body.innerHTML = '<h1 style=""color:#43b581"">{successTitle}</h1><p>{successSub}</p>';
                            }}
                        }}
                    </script>
                </body>
                </html>";

                byte[] buffer = Encoding.UTF8.GetBytes(responseHtml);
                response.ContentType = "text/html";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            else if (request.Url.AbsolutePath == "/token")
            {
                accessToken = request.QueryString["val"];
                response.StatusCode = 200;
                response.Close();
                break;
            }
        }

        listener.Stop();
        return accessToken;
    }
}