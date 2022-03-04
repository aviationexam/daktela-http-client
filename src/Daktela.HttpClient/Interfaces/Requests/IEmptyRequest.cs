using Daktela.HttpClient.Interfaces.Requests.Builder;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface IEmptyRequest : IRequest, IWithPaging<IPagedRequest>, IWithSortable<ISortingRequest>, IWithFilterable<IFilteringRequest>
{
}
