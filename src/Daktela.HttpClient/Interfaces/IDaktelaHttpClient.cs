using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces.Requests;
using System;
using System.Net.Http;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces;

public interface IDaktelaHttpClient : IDisposable
{
    Task<HttpResponseMessage> RawSendAsync(
        HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken = default
    );

    Task<HttpResponseMessage> RawSendAsync(
        HttpRequestMessage httpRequestMessage,
        HttpCompletionOption httpCompletionOption,
        CancellationToken cancellationToken = default
    );

    Task<SingleResponse<T>> GetAsync<T>(
        IHttpResponseParser httpResponseParser,
        string uri,
        JsonTypeInfo<SingleResponse<T>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    ) where T : class;

    Task<ListResponse<T>> GetListAsync<T>(
        IHttpResponseParser httpResponseParser,
        string uri,
        IRequest request,
        JsonTypeInfo<ListResponse<T>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    ) where T : class;

    Task<TResponseContract> PostAsync<TRequest, TResponseContract>(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string uri,
        TRequest request,
        JsonTypeInfo<TRequest> jsonTypeInfoForRequestType,
        JsonTypeInfo<SingleResponse<TResponseContract>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
        where TRequest : class
        where TResponseContract : class;

    Task<TResponseContract> PutAsync<TRequest, TResponseContract>(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string uri,
        TRequest request,
        JsonTypeInfo<TRequest> jsonTypeInfoForRequestType,
        JsonTypeInfo<SingleResponse<TResponseContract>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
        where TRequest : class
        where TResponseContract : class;

    Task DeleteAsync(
        string uri,
        CancellationToken cancellationToken
    );
}
