using Daktela.HttpClient.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations;

public class DaktelaHttpClient : IDaktelaHttpClient
{
    private readonly System.Net.Http.HttpClient _httpClient;

    private readonly bool _leaveHttpClientOpen;

    public DaktelaHttpClient(
        System.Net.Http.HttpClient httpClient,
        bool leaveHttpClientOpen = false
    )
    {
        _httpClient = httpClient;
        _leaveHttpClientOpen = leaveHttpClientOpen;
    }

    public void Dispose()
    {
        if (!_leaveHttpClientOpen)
        {
            _httpClient.Dispose();
        }
    }

    public async Task<T> GetAsync<T>(IHttpResponseParser httpResponseParser, string uri, CancellationToken cancellationToken)
    {
        var uriObject = new Uri(uri, UriKind.Relative);

        using var httpRequestMessage = CreateHttpRequestMessage(HttpMethod.Get, uriObject);

        var httpResponse = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var response = await httpResponseParser.ParseResponseAsync<T>(httpResponse.Content, cancellationToken);

        return response;
    }

    private HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, Uri uri
    ) => new(method, uri);

    private HttpRequestMessage CreateHttpRequestMessage<TBody>(
        HttpMethod method, Uri uri, TBody? body
    ) where TBody : class
    {
        var httpMessage = CreateHttpRequestMessage(method, uri);

        return httpMessage;
    }
}
