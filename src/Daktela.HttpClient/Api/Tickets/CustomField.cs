using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class CustomField
{
    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    public string Name { get; set; } = null!;
}
