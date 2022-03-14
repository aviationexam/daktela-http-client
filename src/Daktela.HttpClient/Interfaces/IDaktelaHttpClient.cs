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

    Task<TResponse> PostAsync<TRequest, TResponse>(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string uri,
        TRequest request,
        CancellationToken cancellationToken
    ) where TRequest : class;

    Task PostAsync<TRequest>(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string uri,
        TRequest request,
        CancellationToken cancellationToken
    ) where TRequest : class;

    Task PutAsync<TRequest>(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string uri,
        TRequest request,
        CancellationToken cancellationToken
    ) where TRequest : class;

    Task DeleteAsync(
        string uri,
        CancellationToken cancellationToken
    );
}
