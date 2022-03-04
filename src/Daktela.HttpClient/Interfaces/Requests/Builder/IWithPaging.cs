using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Queries;

namespace Daktela.HttpClient.Interfaces.Requests.Builder;

public interface IWithPaging<T> where T : class, IPagedQuery
{
    T WithPaging(Paging paging);
}