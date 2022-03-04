using Daktela.HttpClient.Api.Requests;

namespace Daktela.HttpClient.Interfaces.Requests.Builder;

public interface IWithFilterable<T> where T : class, IFilteringQuery
{
    T WithFilter(IFilter filter);
}