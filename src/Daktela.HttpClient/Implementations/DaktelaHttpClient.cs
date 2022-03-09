using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Requests;
using System;
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
        IHttpResponseParser httpResponseParser, string uri, CancellationToken cancellationToken
    ) where T : class
    {
        var uriObject = new Uri(uri, UriKind.Relative);

        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Get, uriObject);

        var httpResponse = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var response = await httpResponseParser.ParseResponseAsync<SingleResponse<T>>(httpResponse.Content, cancellationToken);

        return response;
    }

    public async Task<ListResponse<T>> GetListAsync<T>(
        IHttpResponseParser httpResponseParser,
        string uri,
        IRequest request,
        CancellationToken cancellationToken
    ) where T : class
    {
        var uriObject = new Uri(uri, UriKind.Relative);

        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Get, uriObject, request);

        var httpResponse = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var response = await httpResponseParser.ParseResponseAsync<ListResponse<T>>(httpResponse.Content, cancellationToken);

        return response;
    }

    public async Task DeleteAsync(string uri, CancellationToken cancellationToken)
    {
        var uriObject = new Uri(uri, UriKind.Relative);

        var httpResponse = await _httpClient.DeleteAsync(uriObject, cancellationToken)
            .ConfigureAwait(false);

        if (httpResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return;
        }

        throw new UnexpectedHttpResponseException(uri, httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
    }
}
