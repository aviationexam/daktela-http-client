using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FieldsPagedFilteringRequest(IFields Fields, Paging Paging, IFilter Filters) : IFieldsPagedFilteringRequest
{
    public Paging Paging { get; set; } = Paging;

    public IFieldsPagedSortingFilteringRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new FieldsPagedSortingFilteringRequest(
        Fields,
        Paging,
        sorting,
        Filters
    );
}
