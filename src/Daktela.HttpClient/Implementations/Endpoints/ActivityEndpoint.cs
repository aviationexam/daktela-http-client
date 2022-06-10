using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class ActivityEndpoint : IActivityEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpRequestSerializer _httpRequestSerializer;
    private readonly IHttpResponseParser _httpResponseParser;
    private readonly IPagedResponseProcessor<IActivityEndpoint> _pagedResponseProcessor;

    public ActivityEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        IPagedResponseProcessor<IActivityEndpoint> pagedResponseProcessor
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpRequestSerializer = httpRequestSerializer;
        _httpResponseParser = httpResponseParser;
        _pagedResponseProcessor = pagedResponseProcessor;
    }

    public async Task<ReadActivity> GetActivityAsync(
        string name,
        CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        var contact = await _daktelaHttpClient.GetAsync<ReadActivity>(
            _httpResponseParser,
            $"{IActivityEndpoint.UriPrefix}/{encodedName}{IActivityEndpoint.UriPostfix}",
            cancellationToken
        ).ConfigureAwait(false);

        return contact.Result;
    }

    public IAsyncEnumerable<ReadActivity> GetActivitiesAsync(
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
        ) => await ctx.daktelaHttpClient.GetListAsync<ReadActivity>(
            ctx.httpResponseParser,
            $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.UriPostfix}",
            request,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    public async Task<ReadActivity> CreateActivityAsync(
        CreateActivity activity, CancellationToken cancellationToken
    ) => await _daktelaHttpClient.PostAsync<CreateActivity, ReadActivity>(
        _httpRequestSerializer,
        _httpResponseParser,
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.UriPostfix}",
        activity,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task<ReadActivity> UpdateActivityAsync(
        string name,
        UpdateActivity contact,
        CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        return await _daktelaHttpClient.PutAsync<UpdateActivity, ReadActivity>(
            _httpRequestSerializer,
            _httpResponseParser,
            $"{IActivityEndpoint.UriPrefix}/{encodedName}{IActivityEndpoint.UriPostfix}",
            contact,
            cancellationToken
        ).ConfigureAwait(false);
    }

    public IAsyncEnumerable<TResult> GetActivitiesFieldsAsync<TRequest, TResult>(
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
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
            httpResponseParser = _httpResponseParser
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync<TResult>(
            ctx.httpResponseParser,
            $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.UriPostfix}",
            request,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    #region External relations

    public IAsyncEnumerable<ReadActivityAttachment> GetActivityAttachmentsAsync(
        string name,
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
        ) => await ctx.daktelaHttpClient.GetListAsync<ReadActivityAttachment>(
            ctx.httpResponseParser,
            $"{IActivityEndpoint.UriPrefix}/{ctx.name}/attachments{IActivityEndpoint.UriPostfix}",
            request,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    #endregion
}
