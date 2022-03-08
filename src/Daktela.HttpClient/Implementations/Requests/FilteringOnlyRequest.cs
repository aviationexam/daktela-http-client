using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FilteringOnlyRequest(IFilter Filters) : IFilteringRequest
{
    public IFilteringSortingRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new FilteringSortingRequest(
        Filters,
        sorting
    );

    public IPagedFilteringRequest WithPaging(Paging paging) => new PagedFilteringRequest(
        Filters,
        paging
    );
}
