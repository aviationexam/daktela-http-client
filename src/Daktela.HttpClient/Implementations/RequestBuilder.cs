using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations;

public static class RequestBuilder
{
    public static ISortingRequest CreateSortableQuery(IReadOnlyCollection<Sorting> sorting) => new SortingOnlyRequest(sorting);

    public static IPagedRequest CreatePagedQuery(Paging paging) => new PagingOnlyRequest(paging);

    public static IFilteringRequest CreateFilteringQuery(IFilter filters) => new FilteringOnlyRequest(filters);
}
