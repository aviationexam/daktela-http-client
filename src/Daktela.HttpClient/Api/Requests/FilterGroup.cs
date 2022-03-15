using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public record FilterGroup(
    EFilterLogic Logic,
    IReadOnlyCollection<IFilter> Filters
) : IFilter;
