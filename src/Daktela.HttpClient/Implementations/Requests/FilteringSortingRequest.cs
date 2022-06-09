using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FilteringSortingRequest(IFilter Filters, IReadOnlyCollection<ISorting> Sorting) : IFilteringSortingRequest
{
    public IPagedSortingFilteringRequest WithPaging(Paging paging) => new PagedSortingFilteringRequest(
        paging,
        Sorting,
        Filters
    );

    public IFieldsSortingFilteringRequest WithFields(IFields fields) => new FieldsSortingFilteringRequest(
        fields,
        Sorting,
        Filters
    );
}
