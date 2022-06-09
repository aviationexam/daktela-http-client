using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FieldsFilteringRequest(IFields Fields, IFilter Filters) : IFieldsFilteringRequest
{
    public IFieldsSortingFilteringRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new FieldsSortingFilteringRequest(
        Fields,
        sorting,
        Filters
    );

    public IFieldsPagedFilteringRequest WithPaging(Paging paging) => new FieldsPagedFilteringRequest(
        Fields,
        paging,
        Filters
    );
}
