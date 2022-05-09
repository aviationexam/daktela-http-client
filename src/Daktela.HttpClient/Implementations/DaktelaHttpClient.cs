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

    public async Task<HttpResponseMessage> RawSendAsync(
        HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken
    ) => await _httpClient
        .SendAsync(httpRequestMessage, cancellationToken)
        .ConfigureAwait(false);

    public async Task<HttpResponseMessage> RawSendAsync(
        HttpRequestMessage httpRequestMessage,
        HttpCompletionOption httpCompletionOption,
        CancellationToken cancellationToken
    ) => await _httpClient
        .SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken)
        .ConfigureAwait(false);

    public async Task<SingleResponse<T>> GetAsync<T>(
        IHttpResponseParser httpResponseParser, string path, CancellationToken cancellationToken
    ) where T : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Get, path);

        using var httpResponse = await RawSendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                var response = await httpResponseParser.ParseResponseAsync<SingleResponse<T>>(httpResponse.Content, cancellationToken);

                return response;
            default:
                throw new UnexpectedHttpResponseException(path, httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
        }
    }

    public async Task<ListResponse<T>> GetListAsync<T>(
        IHttpResponseParser httpResponseParser,
        string path,
        IRequest request,
        CancellationToken cancellationToken
    ) where T : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Get, path, request);

        using var httpResponse = await RawSendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                var response = await httpResponseParser.ParseResponseAsync<ListResponse<T>>(httpResponse.Content, cancellationToken);

                return response;
            default:
                throw new UnexpectedHttpResponseException(path, httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
        }
    }

    public async Task<TResponseContract> PostAsync<TRequest, TResponseContract>(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string path,
        TRequest request,
        CancellationToken cancellationToken
    )
        where TRequest : class
        where TResponseContract : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(httpRequestSerializer, HttpMethod.Post, path, request);

        using var httpResponse = await RawSendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.Created:
                var response = await httpResponseParser.ParseResponseAsync<SingleResponse<TResponseContract>>(httpResponse.Content, cancellationToken);

                return response.Result;
            case HttpStatusCode.BadRequest:
                var badRequest = await httpResponseParser.ParseResponseAsync<SingleResponse<TResponseContract>>(httpResponse.Content, cancellationToken);

                throw new BadRequestException<TResponseContract>(badRequest.Result, badRequest.Error);
            default:
                throw new UnexpectedHttpResponseException(path, httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
        }
    }

    public async Task<TResponseContract> PutAsync<TRequest, TResponseContract>(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string path,
        TRequest request,
        CancellationToken cancellationToken
    )
        where TRequest : class
        where TResponseContract : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(httpRequestSerializer, HttpMethod.Put, path, request);

        using var httpResponse = await RawSendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                var response = await httpResponseParser.ParseResponseAsync<SingleResponse<TResponseContract>>(httpResponse.Content, cancellationToken);

                return response.Result;
            case HttpStatusCode.BadRequest:
                var badRequest = await httpResponseParser.ParseResponseAsync<SingleResponse<TResponseContract>>(httpResponse.Content, cancellationToken);

                throw new BadRequestException<TResponseContract>(badRequest.Result, badRequest.Error);
            default:
                throw new UnexpectedHttpResponseException(path, httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
        }
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken)
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Delete, path);

        using var httpResponse = await RawSendAsync(httpRequestMessage, cancellationToken);

        if (httpResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return;
        }

        throw new UnexpectedHttpResponseException(path, httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync(cancellationToken));
    }
}
