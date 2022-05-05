using Daktela.HttpClient.Attributes;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class CreateActivity : UpdateActivity
{
    /// <summary>
    /// Name
    ///
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Type of the activity
    /// </summary>
    [JsonPropertyName("type")]
    [DaktelaRequirement(EOperation.Create)]
    public EActivityType Type { get; set; }

    /// <summary>
    /// User
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    /// Priority
    /// </summary>
    [JsonPropertyName("priority")]
    public int Priority { get; set; }
}
