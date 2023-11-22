using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Requests;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
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
        IHttpResponseParser httpResponseParser,
        string path,
        JsonTypeInfo<SingleResponse<T>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    ) where T : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Get, path);

        using var httpResponse = await RawSendAsync(
            httpRequestMessage,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                try
                {
                    var response = await httpResponseParser.ParseResponseAsync(
                        httpResponse.Content,
                        jsonTypeInfoForResponseType,
                        cancellationToken
                    );

                    return response;
                }
                catch (JsonException e)
                {
                    throw new JsonDaktelaException(e, httpRequestMessage.RequestUri);
                }
            default:
                throw new UnexpectedHttpResponseException(
                    path, httpResponse.StatusCode,
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
        }
    }

    public async Task<ListResponse<T>> GetListAsync<T>(
        IHttpResponseParser httpResponseParser,
        string path,
        IRequest request,
        JsonTypeInfo<ListResponse<T>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    ) where T : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(HttpMethod.Get, path, request);

        using var httpResponse = await RawSendAsync(
            httpRequestMessage,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                try
                {
                    var response = await httpResponseParser.ParseResponseAsync(
                        httpResponse.Content,
                        jsonTypeInfoForResponseType,
                        cancellationToken
                    );

                    return response;
                }
                catch (JsonException e)
                {
                    throw new JsonDaktelaException(e, httpRequestMessage.RequestUri);
                }
            default:
                throw new UnexpectedHttpResponseException(
                    path, httpResponse.StatusCode,
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
        }
    }

    public async Task<TResponseContract> PostAsync<
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicFields |
            DynamicallyAccessedMemberTypes.PublicProperties
        )]
        TRequest, TResponseContract
    >(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string path,
        TRequest request,
        JsonTypeInfo<TRequest> jsonTypeInfoForRequestType,
        JsonTypeInfo<SingleResponse<TResponseContract>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
        where TRequest : class
        where TResponseContract : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            httpRequestSerializer, HttpMethod.Post, path, request, jsonTypeInfoForRequestType
        );

        using var httpResponse = await RawSendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.Created:
                try
                {
                    var response = await httpResponseParser.ParseResponseAsync(
                        httpResponse.Content,
                        jsonTypeInfoForResponseType,
                        cancellationToken
                    );

                    return response.Result;
                }
                catch (JsonException e)
                {
                    throw new JsonDaktelaException(e, httpRequestMessage.RequestUri);
                }
            case HttpStatusCode.BadRequest:
                try
                {
                    var badRequest = await httpResponseParser.ParseResponseAsync(
                        httpResponse.Content,
                        jsonTypeInfoForResponseType,
                        cancellationToken
                    );

                    throw new BadRequestException<TResponseContract>(badRequest.Result, badRequest.Error);
                }
                catch (JsonException e)
                {
                    throw new JsonDaktelaException(e, httpRequestMessage.RequestUri);
                }
            default:
                throw new UnexpectedHttpResponseException(
                    path, httpResponse.StatusCode,
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
        }
    }

    public async Task<TResponseContract> PutAsync<
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicFields |
            DynamicallyAccessedMemberTypes.PublicProperties
        )]
        TRequest, TResponseContract
    >(
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        string path,
        TRequest request,
        JsonTypeInfo<TRequest> jsonTypeInfoForRequestType,
        JsonTypeInfo<SingleResponse<TResponseContract>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
        where TRequest : class
        where TResponseContract : class
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            httpRequestSerializer, HttpMethod.Put, path, request, jsonTypeInfoForRequestType
        );

        using var httpResponse = await RawSendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                try
                {
                    var response = await httpResponseParser.ParseResponseAsync(
                        httpResponse.Content,
                        jsonTypeInfoForResponseType,
                        cancellationToken
                    );

                    return response.Result;
                }
                catch (JsonException e)
                {
                    throw new JsonDaktelaException(e, httpRequestMessage.RequestUri);
                }
            case HttpStatusCode.BadRequest:
                try
                {
                    var badRequest =
                        await httpResponseParser.ParseResponseAsync(
                            httpResponse.Content,
                            jsonTypeInfoForResponseType,
                            cancellationToken
                        );

                    throw new BadRequestException<TResponseContract>(badRequest.Result, badRequest.Error);
                }
                catch (JsonException e)
                {
                    throw new JsonDaktelaException(e, httpRequestMessage.RequestUri);
                }
            default:
                throw new UnexpectedHttpResponseException(
                    path, httpResponse.StatusCode,
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
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

        throw new UnexpectedHttpResponseException(
            path, httpResponse.StatusCode,
            await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false)
        );
    }
}
