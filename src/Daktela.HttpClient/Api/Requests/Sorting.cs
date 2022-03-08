using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public record Sorting : ISorting
{
    [JsonPropertyName("field")]
    public string Field { get; init; } = null!;

    [JsonPropertyName("dir")]
    public ESortDirection Dir { get; init; }
}
