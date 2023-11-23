using Daktela.HttpClient.Interfaces.Requests;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public class EmailActivityDirection : IFieldResult
{
    [JsonPropertyName("direction")]
    public EActivityDirection Direction { get; set; }
}
