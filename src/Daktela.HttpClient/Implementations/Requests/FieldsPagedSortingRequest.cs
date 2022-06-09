using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FieldsPagedSortingRequest(IFields Fields, Paging Paging, IReadOnlyCollection<ISorting> Sorting) : IFieldsPagedSortingRequest
{
    public Paging Paging { get; set; } = Paging;

    public IFieldsPagedSortingFilteringRequest WithFilter(IFilter filter) => new FieldsPagedSortingFilteringRequest(
        Fields,
        Paging,
        Sorting,
        filter
    );
}
