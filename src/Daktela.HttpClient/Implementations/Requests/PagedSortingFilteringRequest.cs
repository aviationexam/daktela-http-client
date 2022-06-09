using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagedSortingFilteringRequest(Paging Paging, IReadOnlyCollection<ISorting> Sorting, IFilter Filters) : IPagedSortingFilteringRequest
{
    public Paging Paging { get; set; } = Paging;

    public IFieldsPagedSortingFilteringRequest WithFields(IFields fields) => new FieldsPagedSortingFilteringRequest(
        fields,
        Paging,
        Sorting,
        Filters
    );
}
