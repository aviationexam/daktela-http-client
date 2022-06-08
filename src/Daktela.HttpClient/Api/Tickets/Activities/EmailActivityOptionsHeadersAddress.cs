using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public class EmailActivityOptionsHeadersAddress
{
    [JsonPropertyName("address")]
    public string Address { get; set; } = null!;

    [JsonPropertyName("display")]
    public string Display { get; set; } = null!;

    [JsonPropertyName("is_group")]
    public bool IsGroup { get; set; }
}