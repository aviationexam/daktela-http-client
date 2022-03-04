using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FilteringSortingRequest(IFilter Filters, IReadOnlyCollection<Sorting> Sorting) : IFilteringSortingRequest
{
    public IPagedSortingFilteringRequest WithPaging(Paging paging) => new PagedSortingFilteringRequest(
        Filters,
        Sorting,
        paging
    );
}
