using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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

    public async Task<SingleResponse<T>> GetAsync<T>(
        IHttpResponseParser httpResponseParser, string uri, CancellationToken cancellationToken
    ) where T : class
    {
        var uriObject = new Uri(uri, UriKind.Relative);

        using var httpRequestMessage = CreateHttpRequestMessage(HttpMethod.Get, uriObject);

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

        using var httpRequestMessage = CreateHttpRequestMessage(HttpMethod.Get, uriObject, request);

        var httpResponse = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        var response = await httpResponseParser.ParseResponseAsync<ListResponse<T>>(httpResponse.Content, cancellationToken);

        return response;
    }

    private HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, Uri uri
    ) => new(method, uri);

    private HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, Uri uri, IRequest request
    )
    {
        var queryDictionary = new NameValueCollection();
        if (request is ISortableQuery { Sorting: { } sorting })
        {
            // TODO sorting
        }

        if (request is IFilteringQuery { Filters: { } filters })
        {
            // TODO filters
        }

        if (request is IPagedQuery { Paging: { } paging })
        {
            queryDictionary.Add("skip", paging.Skip.ToString());
            queryDictionary.Add("take", paging.Take.ToString());
        }

        if (queryDictionary.HasKeys())
        {
            var parsedQuery = HttpUtility.ParseQueryString(uri.Query);

            foreach (string key in queryDictionary.Keys)
            {
                parsedQuery[key] = queryDictionary[key];
            }

            var uriBuilder = new UriBuilder(uri)
            {
                Query = parsedQuery.ToString()
            };

            uri = uriBuilder.Uri;
        }

        return CreateHttpRequestMessage(method, uri);
    }

    private HttpRequestMessage CreateHttpRequestMessage<TBody>(
        HttpMethod method, Uri uri, TBody? body
    ) where TBody : class
    {
        var httpMessage = CreateHttpRequestMessage(method, uri);

        return httpMessage;
    }
}
