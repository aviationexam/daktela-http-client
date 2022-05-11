using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
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
        int name,
        CancellationToken cancellationToken
    )
    {
        var contact = await _daktelaHttpClient.GetAsync<ReadTicket>(
            _httpResponseParser,
            $"{ITicketEndpoint.UriPrefix}/{name}{ITicketEndpoint.UriPostfix}",
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
        ) => await ctx.daktelaHttpClient.GetListAsync<ReadTicket>(
            ctx.httpResponseParser,
            $"{ITicketEndpoint.UriPrefix}{ITicketEndpoint.UriPostfix}",
            request,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    public async Task<ReadTicket> CreateTicketAsync(
        CreateTicket ticket, CancellationToken cancellationToken
    ) => await _daktelaHttpClient.PostAsync<CreateTicket, ReadTicket>(
        _httpRequestSerializer,
        _httpResponseParser,
        $"{ITicketEndpoint.UriPrefix}{ITicketEndpoint.UriPostfix}",
        ticket,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task<ReadTicket> UpdateTicketAsync(
        int name,
        UpdateTicket ticket,
        CancellationToken cancellationToken
    ) => await _daktelaHttpClient.PutAsync<UpdateTicket, ReadTicket>(
        _httpRequestSerializer,
        _httpResponseParser,
        $"{ITicketEndpoint.UriPrefix}/{name}{ITicketEndpoint.UriPostfix}",
        ticket,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task DeleteTicketAsync(
        int name, CancellationToken cancellationToken
    ) => await _daktelaHttpClient.DeleteAsync(
        $"{ITicketEndpoint.UriPrefix}/{name}{ITicketEndpoint.UriPostfix}",
        cancellationToken
    ).ConfigureAwait(false);

    #region External relations

    public IAsyncEnumerable<ReadActivity> GetTicketActivitiesAsync(
        int name,
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
        ) => await ctx.daktelaHttpClient.GetListAsync<ReadActivity>(
            ctx.httpResponseParser,
            $"{ITicketEndpoint.UriPrefix}/{ctx.name}/activities{ITicketEndpoint.UriPostfix}",
            request,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    #endregion
}
