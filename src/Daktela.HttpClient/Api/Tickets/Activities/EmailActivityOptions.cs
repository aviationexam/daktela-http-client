using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public class EmailActivityOptions
{
    [JsonPropertyName("headers")]
    public EmailActivityOptionsHeaders Headers { get; set; } = null!;
}