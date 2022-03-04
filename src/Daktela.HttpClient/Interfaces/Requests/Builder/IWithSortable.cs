using Daktela.HttpClient.Api.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Interfaces.Requests.Builder;

public interface IWithSortable<T> where T : class, ISortableQuery
{
    T WithSortable(ICollection<Sorting> sorting);
}
