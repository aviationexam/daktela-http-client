using Daktela.HttpClient.Api.Requests;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface IPagedQuery : IRequest
{
    Paging Paging { get; }
}
