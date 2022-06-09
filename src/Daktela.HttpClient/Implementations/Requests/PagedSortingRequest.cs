using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagedSortingRequest(Paging Paging, IReadOnlyCollection<ISorting> Sorting) : IPagedSortingRequest
{
    public Paging Paging { get; set; } = Paging;

    public IPagedSortingFilteringRequest WithFilter(IFilter filter) => new PagedSortingFilteringRequest(
        Paging,
        Sorting,
        filter
    );

    public IFieldsPagedSortingRequest WithFields(IFields fields) => new FieldsPagedSortingRequest(
        fields,
        Paging,
        Sorting
    );
}
