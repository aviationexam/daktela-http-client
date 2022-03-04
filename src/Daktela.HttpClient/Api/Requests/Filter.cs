using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public record Filter : IFilter
{
    [JsonPropertyName("field")]
    public string Field { get; init; } = null!;

    [JsonPropertyName("operator")]
    public EFilterOperator Operator { get; init; }

    [JsonPropertyName("value")]
    public string Value { get; init; } = null!;

    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
