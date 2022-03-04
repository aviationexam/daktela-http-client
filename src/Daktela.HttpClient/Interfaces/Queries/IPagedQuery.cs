using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;

namespace Daktela.HttpClient.Interfaces.Queries;

public interface IPagedQuery : IRequest
{
    Paging Paging { get; }
}