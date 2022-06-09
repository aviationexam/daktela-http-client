using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FieldsPagedRequest(IFields Fields, Paging Paging) : IFieldsPagedRequest
{
    public Paging Paging { get; set; } = Paging;

    public IFieldsPagedSortingRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new FieldsPagedSortingRequest(
        Fields,
        Paging,
        sorting
    );

    public IFieldsPagedFilteringRequest WithFilter(IFilter filter) => new FieldsPagedFilteringRequest(
        Fields,
        Paging,
        filter
    );
}
