using Newtonsoft.Json;

namespace RamCleaner.WinForms.Core.Models;

internal class AuthResponse
{
    [JsonProperty("authorized")]
    internal bool Authorized { get; set; }
}