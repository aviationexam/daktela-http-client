using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public class FilterGroup : IFilter
{
    [JsonPropertyName("logic")]
    public EFilterLogic Logic { get; set; }

    [JsonPropertyName("filter")]
    public ICollection<IFilter> Filters { get; set; } = null!;
}
