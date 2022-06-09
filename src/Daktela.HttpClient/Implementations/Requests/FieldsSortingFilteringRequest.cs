using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FieldsSortingFilteringRequest(IFields Fields, IReadOnlyCollection<ISorting> Sorting, IFilter Filters) : IFieldsSortingFilteringRequest
{
    public IFieldsPagedSortingFilteringRequest WithPaging(Paging paging) => new FieldsPagedSortingFilteringRequest(
        Fields,
        paging,
        Sorting,
        Filters
    );
}
