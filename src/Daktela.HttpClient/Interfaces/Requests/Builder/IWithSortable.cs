using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Queries;
using System.Collections.Generic;

namespace Daktela.HttpClient.Interfaces.Requests.Builder;

public interface IWithSortable<T> where T : class, ISortableQuery
{
    T WithSortable(IReadOnlyCollection<Sorting> sorting);
}
