using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests.Builder;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface IFieldsSortingRequest : IFieldsQuery, ISortableQuery, IWithPaging<IFieldsPagedSortingRequest>, IWithFilterable<IFieldsSortingFilteringRequest>;

public interface IFieldsPagedRequest : IFieldsQuery, IPagedQuery, IWithSortable<IFieldsPagedSortingRequest>, IWithFilterable<IFieldsPagedFilteringRequest>;

public interface IFieldsPagedFilteringRequest : IFieldsQuery, IPagedQuery, IFilteringQuery, IWithSortable<IFieldsPagedSortingFilteringRequest>;

public interface IFieldsFilteringRequest : IFieldsQuery, IFilteringQuery, IWithSortable<IFieldsSortingFilteringRequest>, IWithPaging<IFieldsPagedFilteringRequest>;

public interface IFieldsPagedSortingRequest : IFieldsQuery, IPagedQuery, ISortableQuery, IWithFilterable<IFieldsPagedSortingFilteringRequest>;

public interface IFieldsSortingFilteringRequest : IFieldsQuery, ISortableQuery, IFilteringQuery, IWithPaging<IFieldsPagedSortingFilteringRequest>;

public interface IFieldsPagedSortingFilteringRequest : IFieldsQuery, IPagedQuery, ISortableQuery, IFilteringQuery;
