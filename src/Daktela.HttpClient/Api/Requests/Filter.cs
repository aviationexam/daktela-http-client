using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public class Filter : IFilter
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = null!;

    [JsonPropertyName("operator")]
    public EFilterOperator Operator { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; } = null!;

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
