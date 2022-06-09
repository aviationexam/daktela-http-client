using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagingOnlyRequest(Paging Paging) : IPagedRequest
{
    public Paging Paging { get; set; } = Paging;

    public IPagedSortingRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new PagedSortingRequest(
        Paging,
        sorting
    );

    public IPagedFilteringRequest WithFilter(IFilter filters) => new PagedFilteringRequest(
        filters,
        Paging
    );

    public IFieldsPagedRequest WithFields(IFields fields) => new FieldsPagedRequest(
        fields,
        Paging
    );
}
