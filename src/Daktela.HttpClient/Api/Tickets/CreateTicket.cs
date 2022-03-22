using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class CreateTicket : UpdateTicket
{
    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
