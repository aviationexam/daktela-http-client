using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FieldsOnlyRequest(IFields Fields) : IFieldsRequest
{
    public IFieldsSortingRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new FieldsSortingRequest(
        Fields,
        sorting
    );

    public IFieldsPagedRequest WithPaging(Paging paging) => new FieldsPagedRequest(
        Fields,
        paging
    );

    public IFieldsFilteringRequest WithFilter(IFilter filter) => new FieldsFilteringRequest(
        Fields,
        filter
    );
}
