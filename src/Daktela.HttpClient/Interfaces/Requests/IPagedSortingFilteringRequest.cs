using Daktela.HttpClient.Interfaces.Queries;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface IPagedSortingFilteringRequest : ISortableQuery, IPagedQuery, IFilteringQuery
{
}
