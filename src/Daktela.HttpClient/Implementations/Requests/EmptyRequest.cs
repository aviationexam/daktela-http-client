using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record EmptyRequest : IEmptyRequest
{
    public IPagedRequest WithPaging(Paging paging) => new PagingOnlyRequest(paging);

    public ISortingRequest WithSortable(IReadOnlyCollection<ISorting> sorting) => new SortingOnlyRequest(sorting);

    public IFilteringRequest WithFilter(IFilter filter) => new FilteringOnlyRequest(filter);

    public IFieldsRequest WithFields(IFields fields) => new FieldsOnlyRequest(fields);
}
