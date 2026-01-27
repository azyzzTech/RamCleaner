using Newtonsoft.Json;

namespace RamCleaner.WinForms.Core.Models;

/// <summary>
/// Model representing a minimal response from the auth API indicating whether the user is authorized.
/// </summary>
internal class AuthResponse
{
    [JsonProperty("authorized")]
    internal bool Authorized { get; set; }
}
