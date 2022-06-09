using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations;

public static class RequestBuilder
{
    public static IEmptyRequest CreateEmpty() => new EmptyRequest();

    public static ISortingRequest CreateSortable(IReadOnlyCollection<ISorting> sorting) => new SortingOnlyRequest(sorting);

    public static ISortingRequest CreateSortable(ISorting sorting) => CreateSortable(new[] { sorting });

    public static IPagedRequest CreatePaged(Paging paging) => new PagingOnlyRequest(paging);

    public static IFilteringRequest CreateFiltering(IFilter filters) => new FilteringOnlyRequest(filters);

    public static IFieldsRequest CreateFields(IFields fields) => new FieldsOnlyRequest(fields);
}
