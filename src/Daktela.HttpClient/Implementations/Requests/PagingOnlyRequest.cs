using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagingOnlyRequest(Paging Paging) : IPagedRequest
{
    public Paging Paging { get; set; } = Paging;

    public IPagedSortingRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new PagedSortingRequest(
        sorting,
        Paging
    );

    public IPagedFilteringRequest WithFilter(IFilter filters) => new PagedFilteringRequest(
        filters,
        Paging
    );
}
