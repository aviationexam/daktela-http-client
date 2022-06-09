using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record FieldsPagedSortingFilteringRequest(IFields Fields, Paging Paging, IReadOnlyCollection<ISorting> Sorting, IFilter Filters) : IFieldsPagedSortingFilteringRequest
{
    public Paging Paging { get; set; } = Paging;
}
