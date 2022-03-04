using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Queries;

namespace Daktela.HttpClient.Interfaces.Requests.Builder;

public interface IWithFilterable<T> where T : class, IFilteringQuery
{
    T WithFilter(IFilter filter);
}