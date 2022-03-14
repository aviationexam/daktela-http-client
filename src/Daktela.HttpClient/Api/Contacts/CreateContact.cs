using Daktela.HttpClient.Attributes;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Contacts;

public class CreateContact : UpdateContact
{
    /// <summary>
    /// Unique name
    /// </summary>
    [DaktelaRequirement(EOperation.Create)]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}
