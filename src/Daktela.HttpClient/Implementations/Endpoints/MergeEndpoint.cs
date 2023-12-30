using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Accounts;
using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Merge;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class MergeEndpoint : IMergeEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpRequestFactory _httpRequestFactory;
    private readonly IHttpResponseParser _httpResponseParser;

    public MergeEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpRequestFactory httpRequestFactory,
        IHttpResponseParser httpResponseParser
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpRequestFactory = httpRequestFactory;
        _httpResponseParser = httpResponseParser;
    }

    public Task<ReadContact> MergeContactsAsync(
        IReadOnlyCollection<string> items,
        CancellationToken cancellationToken
    ) => MergeAsync(
        EMergeType.Contacts,
        items,
        DaktelaJsonSerializerContext.Default.SingleResponseReadContact,
        cancellationToken
    );

    public Task<ReadAccount> MergeAccountsAsync(
        IReadOnlyCollection<string> items,
        CancellationToken cancellationToken
    ) => MergeAsync(
        EMergeType.Accounts,
        items,
        DaktelaJsonSerializerContext.Default.SingleResponseReadAccount,
        cancellationToken
    );

    public Task<ReadTicket> MergeTicketsAsync(
        IReadOnlyCollection<string> items,
        CancellationToken cancellationToken
    ) => MergeAsync(
        EMergeType.Tickets,
        items,
        DaktelaJsonSerializerContext.Default.SingleResponseReadTicket,
        cancellationToken
    );

    private string GePath(EMergeType type) => type switch
    {
        EMergeType.Contacts => $"{IMergeEndpoint.UriPrefix}Contacts{IMergeEndpoint.UriPostfix}",
        EMergeType.Accounts => $"{IMergeEndpoint.UriPrefix}Accounts{IMergeEndpoint.UriPostfix}",
        EMergeType.Tickets => $"{IMergeEndpoint.UriPrefix}Tickets{IMergeEndpoint.UriPostfix}",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    private string GetTypeAsQueryString(EMergeType type) => type switch
    {
        EMergeType.Contacts => "contacts",
        EMergeType.Accounts => "accounts",
        EMergeType.Tickets => "tickets",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    public async Task<T> MergeAsync<T>(
        EMergeType type,
        IReadOnlyCollection<string> items,
        JsonTypeInfo<SingleResponse<T>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    ) where T : class
    {
        var path = GePath(type);
        var typeAsQueryString = GetTypeAsQueryString(type);

        var queryParameters = new NameValueCollection();
        for (var i = 0; i < items.Count; i++)
        {
            queryParameters.Add($"{typeAsQueryString}[{i}]", items.ElementAt(i));
        }

        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Post,
            path,
            queryParameters
        );

        using var httpResponse = await _daktelaHttpClient.RawSendAsync(
            httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken
        ).ConfigureAwait(false);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                try
                {
                    var response = await _httpResponseParser.ParseResponseAsync(
                        httpResponse.Content,
                        jsonTypeInfoForResponseType,
                        cancellationToken
                    ).ConfigureAwait(false);

                    return response.Result;
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
}
