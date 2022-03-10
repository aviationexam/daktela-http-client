using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Requests;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations;

public class DaktelaHttpClient : IDaktelaHttpClient
{
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly IHttpRequestFactory _httpRequestFactory;

    private readonly bool _leaveHttpClientOpen;

    public DaktelaHttpClient(
        System.Net.Http.HttpClient httpClient,
        IHttpRequestFactory httpRequestFactory,
        bool leaveHttpClientOpen = false
    )
    {
        _httpClient = httpClient;
        _httpRequestFactory = httpRequestFactory;
        _leaveHttpClientOpen = leaveHttpClientOpen;
    }

    public void Dispose()
    {
        if (!_leaveHttpClientOpen)
        {
            _httpClient.Dispose();
        }
    }

    public async Task<SingleResponse<T>> GetAsync<T>(
        IHttpResponseParser httpResponseParser, string path, CancellationToken cancellationToken
    ) where T : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Get, path);

        using var httpResponse = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var response = await httpResponseParser.ParseResponseAsync<SingleResponse<T>>(httpResponse.Content, cancellationToken);

        return response;
    }

    public async Task<ListResponse<T>> GetListAsync<T>(
        IHttpResponseParser httpResponseParser,
        string path,
        IRequest request,
        CancellationToken cancellationToken
    ) where T : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Get, path, request);

        using var httpResponse = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var response = await httpResponseParser.ParseResponseAsync<ListResponse<T>>(httpResponse.Content, cancellationToken);

        return response;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string path,
        TRequest request,
        CancellationToken cancellationToken
    ) where TRequest : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(httpRequestSerializer, HttpMethod.Post, path, request);

        using var httpResponse = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var response = await httpResponseParser.ParseResponseAsync<TResponse>(httpResponse.Content, cancellationToken);

        return response;
    }

    public async Task PostAsync<TRequest>(
        IHttpRequestSerializer httpRequestSerializer,
        string path,
        TRequest request,
        CancellationToken cancellationToken
    ) where TRequest : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(httpRequestSerializer, HttpMethod.Post, path, request);

        using var httpResponse = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        if (httpResponse.StatusCode == HttpStatusCode.Created)
        {
            return;
        }

        throw new UnexpectedHttpResponseException(path, httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Delete, path);

        using var httpResponse = await _httpClient.SendAsync(httpRequestMessage, cancellationToken)
            .ConfigureAwait(false);

        if (httpResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return;
        }

        throw new UnexpectedHttpResponseException(path, httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
    }
}
