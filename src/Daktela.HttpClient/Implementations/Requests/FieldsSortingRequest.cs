using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FieldsSortingRequest(IFields Fields, IReadOnlyCollection<ISorting> Sorting) : IFieldsSortingRequest
{
    public IFieldsPagedSortingRequest WithPaging(Paging paging) => new FieldsPagedSortingRequest(
        Fields,
        paging,
        Sorting
    );

    public IFieldsSortingFilteringRequest WithFilter(IFilter filter) => new FieldsSortingFilteringRequest(
        Fields,
        Sorting,
        filter
    );
}
