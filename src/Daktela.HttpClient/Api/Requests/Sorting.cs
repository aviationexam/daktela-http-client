using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public record Sorting(string Field, ESortDirection Dir) : ISorting
{
    [JsonPropertyName("field")]
    public string Field { get; } = Field;

    [JsonPropertyName("dir")]
    public ESortDirection Dir { get; } = Dir;
}
