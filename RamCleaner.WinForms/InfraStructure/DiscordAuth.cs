using Newtonsoft.Json;
using RamCleaner.WinForms.Core.Models;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

namespace RamCleaner.WinForms.InfraStructure;

using RamCleaner.WinForms.Core.Services;

internal class DiscordAuth : IAuthService
{
    private const string ClientId = "1463542451460636828";
    private const string RedirectUri = "http://localhost:5000/";
    private const string ApiBaseUrl = "https://ram-cleaner-dc-auth-api.onrender.com";
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly ILogger<DiscordAuth>? _logger;

    public DiscordAuth(System.Net.Http.HttpClient httpClient, ILogger<DiscordAuth>? logger = null)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> FullAuthFlowAsync(System.Threading.CancellationToken ct = default)
    {
        try
        {
            DateTime lastAuth = Properties.Settings.Default.LastAuthDate;
            bool isStillValid = (DateTime.Now - lastAuth).TotalDays < 7;
            bool wasAuthorized = Properties.Settings.Default.IsAuthorized;

            if (wasAuthorized && isStillValid)
            {
                return true;
            }

            string authUrl = $"https://discord.com/api/oauth2/authorize?client_id={ClientId}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&response_type=token&scope=identify";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = authUrl,
                UseShellExecute = true
            };
            Process.Start(psi);

            string accessToken = await ListenForAccessToken(ct);

            if (string.IsNullOrEmpty(accessToken))
                return false;

            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/check-auth?access_token={accessToken}", ct);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync(ct);
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
        catch (OperationCanceledException)
        {
            _logger?.LogInformation("Auth flow canceled");
            return false;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Auth flow failed");
            return false;
        }
    }

    private async Task<string> ListenForAccessToken(System.Threading.CancellationToken ct = default)
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add(RedirectUri);
        listener.Start();

        string? accessToken = null;

        try
        {
            while (accessToken == null && !ct.IsCancellationRequested)
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;

                if (request.Url.AbsolutePath == "/")
                {
                    string title = "Authorizing...";
                    string subText = "Please wait, you are being redirected.";
                    string successTitle = "Success!";
                    string successSub = "Login successful. You can close this tab.";

                    string responseHtml = $@"<html>
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
                                document.body.innerHTML = '<h1 style=""""color:#43b581"""">{successTitle}</h1><p>{successSub}</p>';
                            }}
                        }}
                    </script>
                </body>
                </html>";

                    // Fix quotes for the inner HTML double-quotes
                    responseHtml = responseHtml.Replace("\"\"\"\"", "\"");

                    byte[] buffer = Encoding.UTF8.GetBytes(responseHtml);
                    response.ContentType = "text/html";
                    response.ContentLength64 = buffer.Length;
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, ct);
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
        }
        catch (OperationCanceledException)
        {
            _logger?.LogInformation("ListenForAccessToken canceled");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "ListenForAccessToken failed");
        }
        finally
        {
            try { listener.Stop(); } catch { }
        }

        return accessToken ?? string.Empty;
    }
}
