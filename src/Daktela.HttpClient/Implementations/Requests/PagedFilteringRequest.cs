using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagedFilteringRequest(IFilter Filters, Paging Paging) : IPagedFilteringRequest
{
    public Paging Paging { get; set; } = Paging;

    public IPagedSortingFilteringRequest WithSortable(IReadOnlyCollection<Sorting> sorting) => new PagedSortingFilteringRequest(
        Filters,
        sorting,
        Paging
    );
}
