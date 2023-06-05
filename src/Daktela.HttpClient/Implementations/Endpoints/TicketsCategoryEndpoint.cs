using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using System.Threading;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class TicketsCategoryEndpoint : ITicketsCategoryEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpResponseParser _httpResponseParser;
    private readonly IPagedResponseProcessor<ITicketsCategoryEndpoint> _pagedResponseProcessor;

    public TicketsCategoryEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpResponseParser httpResponseParser,
        IPagedResponseProcessor<ITicketsCategoryEndpoint> pagedResponseProcessor
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpResponseParser = httpResponseParser;
        _pagedResponseProcessor = pagedResponseProcessor;
    }

    public IAsyncEnumerable<Category> GetTicketsCategoriesAsync(
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
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync(
            ctx.httpResponseParser,
            $"{ITicketsCategoryEndpoint.UriPrefix}{ITicketsCategoryEndpoint.UriPostfix}",
            request,
            DaktelaJsonSerializerContext.CustomConverters.ListResponseCategory,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);
}
