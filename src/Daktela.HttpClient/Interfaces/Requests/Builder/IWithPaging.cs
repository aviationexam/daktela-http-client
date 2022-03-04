using Daktela.HttpClient.Api.Requests;

namespace Daktela.HttpClient.Interfaces.Requests.Builder;

public interface IWithPaging<T> where T : class, IPagedQuery
{
    T WithPaging(Paging paging);
}