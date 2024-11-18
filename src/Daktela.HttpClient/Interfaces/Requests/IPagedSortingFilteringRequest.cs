using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests.Builder;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface IPagedSortingFilteringRequest : ISortableQuery, IPagedQuery, IFilteringQuery, IWithFields<IFieldsPagedSortingFilteringRequest>;
