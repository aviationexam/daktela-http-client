using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagedSortingRequest(IReadOnlyCollection<Sorting> Sorting, Paging Paging) : IPagedSortingRequest
{
    public Paging Paging { get; set; } = Paging;

    public IPagedSortingFilteringRequest WithFilter(IFilter filter) => new PagedSortingFilteringRequest(
        filter,
        Sorting,
        Paging
    );
}
