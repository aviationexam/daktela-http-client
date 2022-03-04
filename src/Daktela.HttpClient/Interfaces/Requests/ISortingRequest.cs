using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests.Builder;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface ISortingRequest : ISortableQuery, IWithFilterable<IFilteringSortingRequest>, IWithPaging<IPagedSortingRequest>
{
}
