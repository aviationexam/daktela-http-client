using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagedFilteringRequest(IFilter Filters, Paging Paging) : IPagedFilteringRequest
{
    public Paging Paging { get; set; } = Paging;

    public IPagedSortingFilteringRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new PagedSortingFilteringRequest(
        Paging,
        sorting,
        Filters
    );

    public IFieldsPagedFilteringRequest WithFields(IFields fields) => new FieldsPagedFilteringRequest(
        fields,
        Paging,
        Filters
    );
}
