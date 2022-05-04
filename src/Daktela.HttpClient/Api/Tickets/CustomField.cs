using Daktela.HttpClient.Attributes;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class CustomField
{
    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Name { get; set; } = null!;
}
