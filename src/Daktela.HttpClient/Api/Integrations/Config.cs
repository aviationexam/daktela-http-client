using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Integrations;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/integrationsconfigs">Config</a>
/// </summary>
public class Config
{
    /// <summary>
    /// name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Title
    /// </summary>
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Integration
    /// </summary>
    [JsonPropertyName("integration")]
    public Integration Integration { get; set; } = null!;

    /// <summary>
    /// Active
    /// </summary>
    [JsonPropertyName("active")]
    public bool Active { get; set; }

    /// <summary>
    /// Authorization
    /// </summary>
    [JsonPropertyName("auth")]
    public object? Auth { get; set; }

    /// <summary>
    /// Configuration
    /// </summary>
    [JsonPropertyName("config")]
    public object? Configuration { get; set; }

    /// <summary>
    /// Error
    /// </summary>
    [JsonPropertyName("error")]
    public object? Error { get; set; }
}
