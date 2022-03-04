using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests.Builder;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface IFilteringRequest : IFilteringQuery, IWithSortable<IFilteringSortingRequest>, IWithPaging<IPagedFilteringRequest>
{
}
