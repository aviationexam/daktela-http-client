using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Threading;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class TicketsCategoryEndpoint(
    IDaktelaHttpClient daktelaHttpClient,
    IHttpResponseParser httpResponseParser,
    IPagedResponseProcessor<ITicketsCategoryEndpoint> pagedResponseProcessor
) : ITicketsCategoryEndpoint
{
    public IAsyncEnumerable<Category> GetTicketsCategoriesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient,
            httpResponseParser,
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
            DaktelaJsonSerializerContext.Default.ListResponseCategory,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);
}
