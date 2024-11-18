using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Queries;
using System.Collections.Generic;

namespace Daktela.HttpClient.Interfaces.Requests.Builder;

public interface IWithSortable<T> where T : class, ISortableQuery
{
    T WithSortable(IReadOnlyCollection<ISorting> sorting);

    T WithSortable(ISorting sorting) => WithSortable([sorting]);
}
