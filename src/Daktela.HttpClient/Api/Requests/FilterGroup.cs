using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public record FilterGroup : IFilter
{
    [JsonPropertyName("logic")]
    public EFilterLogic Logic { get; init; }

    [JsonPropertyName("filter")]
    public ICollection<IFilter> Filters { get; init; } = null!;
}
