using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagedSortingFilteringRequest(IFilter Filters, IReadOnlyCollection<ISorting> Sorting, Paging Paging) : IPagedSortingFilteringRequest
{
    public Paging Paging { get; set; } = Paging;
}
