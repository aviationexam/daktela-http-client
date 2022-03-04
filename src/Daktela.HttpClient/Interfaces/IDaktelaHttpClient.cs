using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces;

public interface IDaktelaHttpClient : IDisposable
{
    Task<SingleResponse<T>> GetAsync<T>(
        IHttpResponseParser httpResponseParser,
        string uri,
        CancellationToken cancellationToken
    ) where T : class;

    Task<ListResponse<T>> GetListAsync<T>(
        IHttpResponseParser httpResponseParser,
        string uri,
        IRequest request,
        CancellationToken cancellationToken
    ) where T : class;
}
