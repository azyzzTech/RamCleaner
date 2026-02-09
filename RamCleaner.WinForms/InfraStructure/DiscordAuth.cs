using Newtonsoft.Json;
using RamCleaner.WinForms.Core.Models;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

namespace RamCleaner.WinForms.InfraStructure;

using RamCleaner.WinForms.Core.Services;

/// <summary>
/// Handles Discord-based authentication flow using OAuth2. Opens a browser
/// to Discord's authorization URL and listens locally to capture the access token.
/// </summary>
internal class DiscordAuth : IAuthService
{
    private const string ClientId = "1463542451460636828";
    private const string RedirectUri = "http://localhost:5000/";
    private const string ApiBaseUrl = "https://sl-dc-auth-api.vercel.app/";
    private readonly System.Net.Http.IHttpClientFactory _httpFactory;
    private readonly ILogger<DiscordAuth>? _logger;

    /// <summary>
    /// Creates a new DiscordAuth with the provided HTTP factory and optional logger.
    /// </summary>
    public DiscordAuth(System.Net.Http.IHttpClientFactory httpFactory, ILogger<DiscordAuth>? logger = null)
    {
        _httpFactory = httpFactory ?? throw new ArgumentNullException(nameof(httpFactory));
        _logger = logger;
    }

    /// <summary>
    /// Validates stored authorization without requiring interactive flow.
    /// Returns true if the stored settings indicate authorization within last 7 days.
    /// </summary>
    public Task<bool> ValidateAuthStatusAsync(System.Threading.CancellationToken ct = default)
    {
        try
        {
            DateTime lastAuth = Properties.Settings.Default.LastAuthDate;
            bool isStillValid = (DateTime.Now - lastAuth).TotalDays < 7;
            bool wasAuthorized = Properties.Settings.Default.IsAuthorized;

            return Task.FromResult(wasAuthorized && isStillValid);
        }
        catch (Exception ex)
        {
            _logger?.LogDebug(ex, "ValidateAuthStatusAsync failed");
            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// Executes the full OAuth flow and validates authorization against the remote API.
    /// Returns true when authorization is successful and within the configured validity period.
    /// </summary>
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

            var client = _httpFactory.CreateClient();
            var response = await client.GetAsync($"{ApiBaseUrl}/check-auth?access_token={accessToken}", ct);

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

    /// <summary>
    /// Resets the authentication status and clears stored credentials.
    /// </summary>
    public void Logout()
    {
        Properties.Settings.Default.LastAuthDate = DateTime.MinValue;
        Properties.Settings.Default.IsAuthorized = false;
        Properties.Settings.Default.Save();
    }

    /// <summary>
    /// Starts a local HTTP listener to capture the access token returned in the browser redirect.
    /// </summary>
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
