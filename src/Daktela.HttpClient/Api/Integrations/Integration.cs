using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Integrations;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/integrations">Integration</a>
/// </summary>
public class Integration
{
    /// <summary>
    /// name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Type
    /// </summary>
    [JsonPropertyName("type")]
    public EType Type { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Auth info
    /// </summary>
    [JsonPropertyName("authInfo")]
    public string AuthInfo { get; set; } = null!;

    /// <summary>
    /// Image name
    /// </summary>
    [JsonPropertyName("imageName")]
    public string ImageName { get; set; } = null!;

    /// <summary>
    /// Icon name
    /// </summary>
    [JsonPropertyName("icon")]
    public string Icon { get; set; } = null!;

    /// <summary>
    /// Active
    /// </summary>
    [JsonPropertyName("active")]
    public bool Active { get; set; }

    /// <summary>
    /// Authorization
    /// </summary>
    [JsonPropertyName("auth")]
    public bool Auth { get; set; }

    /// <summary>
    /// Configuration
    /// </summary>
    [JsonPropertyName("config")]
    public bool Config { get; set; }

    /// <summary>
    /// Error
    /// </summary>
    [JsonPropertyName("error")]
    public object? Error { get; set; }
}
