using Daktela.HttpClient.Interfaces.Requests;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public class ActivityDirection : IFieldResult
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("item")]
    public EmailActivityDirection? Item { get; set; }
}
