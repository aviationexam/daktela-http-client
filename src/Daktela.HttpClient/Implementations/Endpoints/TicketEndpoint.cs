using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class TicketEndpoint : ITicketEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpRequestSerializer _httpRequestSerializer;
    private readonly IHttpResponseParser _httpResponseParser;
    private readonly IPagedResponseProcessor<ITicketEndpoint> _pagedResponseProcessor;

    public TicketEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        IPagedResponseProcessor<ITicketEndpoint> pagedResponseProcessor
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpRequestSerializer = httpRequestSerializer;
        _httpResponseParser = httpResponseParser;
        _pagedResponseProcessor = pagedResponseProcessor;
    }

    public async Task<ReadTicket> GetTicketAsync(
        long name,
        CancellationToken cancellationToken
    )
    {
        var contact = await _daktelaHttpClient.GetAsync(
            _httpResponseParser,
            $"{ITicketEndpoint.UriPrefix}/{name}{ITicketEndpoint.UriPostfix}",
            DaktelaJsonSerializerContext.Default.SingleResponseReadTicket,
            cancellationToken
        ).ConfigureAwait(false);

        return contact.Result;
    }

    public IAsyncEnumerable<ReadTicket> GetTicketsAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => _pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient = _daktelaHttpClient,
            httpResponseParser = _httpResponseParser
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync(
            ctx.httpResponseParser,
            $"{ITicketEndpoint.UriPrefix}{ITicketEndpoint.UriPostfix}",
            request,
            DaktelaJsonSerializerContext.Default.ListResponseReadTicket,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    public async Task<ReadTicket> CreateTicketAsync(
        CreateTicket ticket, CancellationToken cancellationToken
    ) => await _daktelaHttpClient.PostAsync(
        _httpRequestSerializer,
        _httpResponseParser,
        $"{ITicketEndpoint.UriPrefix}{ITicketEndpoint.UriPostfix}",
        ticket,
        DaktelaJsonSerializerContext.Default.CreateTicket,
        DaktelaJsonSerializerContext.Default.SingleResponseReadTicket,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task<ReadTicket> UpdateTicketAsync(
        long name,
        UpdateTicket ticket,
        CancellationToken cancellationToken
    ) => await _daktelaHttpClient.PutAsync(
        _httpRequestSerializer,
        _httpResponseParser,
        $"{ITicketEndpoint.UriPrefix}/{name}{ITicketEndpoint.UriPostfix}",
        ticket,
        DaktelaJsonSerializerContext.Default.UpdateTicket,
        DaktelaJsonSerializerContext.Default.SingleResponseReadTicket,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task DeleteTicketAsync(
        long name, CancellationToken cancellationToken
    ) => await _daktelaHttpClient.DeleteAsync(
        $"{ITicketEndpoint.UriPrefix}/{name}{ITicketEndpoint.UriPostfix}",
        cancellationToken
    ).ConfigureAwait(false);

    public IAsyncEnumerable<TResult> GetTicketsFieldsAsync<TRequest, TResult>(
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        JsonTypeInfo<ListResponse<TResult>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
        where TRequest : IRequest, IFieldsQuery
        where TResult : class, IFieldResult => _pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient = _daktelaHttpClient,
            httpResponseParser = _httpResponseParser,
            jsonTypeInfoForResponseType,
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync(
            ctx.httpResponseParser,
            $"{ITicketEndpoint.UriPrefix}{ITicketEndpoint.UriPostfix}",
            request,
            ctx.jsonTypeInfoForResponseType,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    #region External relations

    public IAsyncEnumerable<ReadActivity> GetTicketActivitiesAsync(
        long name,
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => _pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient = _daktelaHttpClient,
            httpResponseParser = _httpResponseParser,
            name,
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync(
            ctx.httpResponseParser,
            $"{ITicketEndpoint.UriPrefix}/{ctx.name}/activities{ITicketEndpoint.UriPostfix}",
            request,
            DaktelaJsonSerializerContext.Default.ListResponseReadActivity,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    public IAsyncEnumerable<TResult> GetTicketActivitiesFieldsAsync<TRequest, TResult>(
        long name,
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        JsonTypeInfo<ListResponse<TResult>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
        where TRequest : IRequest, IFieldsQuery
        where TResult : class, IFieldResult => _pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient = _daktelaHttpClient,
            httpResponseParser = _httpResponseParser,
            name,
            jsonTypeInfoForResponseType,
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync(
            ctx.httpResponseParser,
            $"{ITicketEndpoint.UriPrefix}/{ctx.name}/activities{ITicketEndpoint.UriPostfix}",
            request,
            ctx.jsonTypeInfoForResponseType,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    #endregion
}
