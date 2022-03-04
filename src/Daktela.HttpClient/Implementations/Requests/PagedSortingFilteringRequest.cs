using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Implementations.Requests;

internal record PagedSortingFilteringRequest(IFilter Filters, IReadOnlyCollection<Sorting> Sorting, Paging Paging) : IPagedSortingFilteringRequest;
