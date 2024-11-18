using System.Collections.Generic;

namespace Daktela.HttpClient.Api.Requests;

public record FilterGroup(
    EFilterLogic Logic,
    IReadOnlyCollection<IFilter> Filters
) : IFilter;
