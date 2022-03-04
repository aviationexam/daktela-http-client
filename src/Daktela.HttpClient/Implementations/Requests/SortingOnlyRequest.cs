using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record SortingOnlyRequest(IReadOnlyCollection<Sorting> Sorting) : ISortingRequest
{
    public IFilteringSortingRequest WithFilter(IFilter filters) => new FilteringSortingRequest(
        filters,
        Sorting
    );

    public IPagedSortingRequest WithPaging(Paging paging) => new PagedSortingRequest(
        Sorting,
        paging
    );
}
