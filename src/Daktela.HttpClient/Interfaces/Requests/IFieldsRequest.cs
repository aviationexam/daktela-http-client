using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests.Builder;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface IFieldsRequest : IFieldsQuery, IWithSortable<IFieldsSortingRequest>, IWithPaging<IFieldsPagedRequest>, IWithFilterable<IFieldsFilteringRequest>;
