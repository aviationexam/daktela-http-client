using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations.Requests;
using Daktela.HttpClient.Interfaces.Queries;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations;

public static class RequestBuilder
{
    public static ISortableQuery CreateSortableQuery(IReadOnlyCollection<Sorting> sorting) => new SortingOnlyRequest(sorting);

    public static IPagedQuery CreatePagedQuery(Paging paging) => new PagingOnlyRequest(paging);

    public static IFilteringQuery CreateFilteringQuery(IFilter filters) => new FilteringOnlyRequest(filters);
}
