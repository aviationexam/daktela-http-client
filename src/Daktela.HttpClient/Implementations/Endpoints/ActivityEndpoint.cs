using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Api.Tickets.Activities;
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
using System.Web;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class ActivityEndpoint(
    IDaktelaHttpClient daktelaHttpClient,
    IHttpRequestSerializer httpRequestSerializer,
    IHttpResponseParser httpResponseParser,
    IPagedResponseProcessor<IActivityEndpoint> pagedResponseProcessor
) : IActivityEndpoint
{
    public async Task<ReadActivity> GetActivityAsync(
        string name,
        CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        var contact = await daktelaHttpClient.GetAsync(
            httpResponseParser,
            $"{IActivityEndpoint.UriPrefix}/{encodedName}{IActivityEndpoint.UriPostfix}",
            DaktelaJsonSerializerContext.Default.SingleResponseReadActivity,
            cancellationToken
        ).ConfigureAwait(false);

        return contact.Result;
    }

    public IAsyncEnumerable<ReadActivity> GetActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseReadActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public async Task<ReadActivity> CreateActivityAsync(
        CreateActivity activity, CancellationToken cancellationToken
    ) => await daktelaHttpClient.PostAsync(
        httpRequestSerializer,
        httpResponseParser,
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.UriPostfix}",
        activity,
        DaktelaJsonSerializerContext.Default.CreateActivity,
        DaktelaJsonSerializerContext.Default.SingleResponseReadActivity,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task<ReadActivity> UpdateActivityAsync(
        string name,
        UpdateActivity contact,
        CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        return await daktelaHttpClient.PutAsync(
            httpRequestSerializer,
            httpResponseParser,
            $"{IActivityEndpoint.UriPrefix}/{encodedName}{IActivityEndpoint.UriPostfix}",
            contact,
            DaktelaJsonSerializerContext.Default.UpdateActivity,
            DaktelaJsonSerializerContext.Default.SingleResponseReadActivity,
            cancellationToken
        ).ConfigureAwait(false);
    }

    public IAsyncEnumerable<TResult> GetActivitiesFieldsAsync<TRequest, TResult>(
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        JsonTypeInfo<ListResponse<TResult>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
        where TRequest : IRequest, IFieldsQuery
        where TResult : class, IFieldResult => pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient,
            httpResponseParser,
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
            $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.UriPostfix}",
            request,
            ctx.jsonTypeInfoForResponseType,
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
    ) => pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient,
            httpResponseParser,
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
            $"{IActivityEndpoint.UriPrefix}/{ctx.name}/attachments{IActivityEndpoint.UriPostfix}",
            request,
            DaktelaJsonSerializerContext.Default.ListResponseReadActivityAttachment,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    #endregion

    #region Activity items

    public IAsyncEnumerable<CallActivity> GetCallActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.CallSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseCallActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<EmailActivity> GetEmailActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.EmailSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseEmailActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<FacebookCommentActivity> GetFacebookCommentActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.FacebookCommentSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseFacebookCommentActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<FacebookMessengerActivity> GetFacebookMessengerActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.FacebookMessengerSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseFacebookMessengerActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<InstagramCommentActivity> GetInstagramCommentActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.InstagramCommentSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseInstagramCommentActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<InstagramDirectMessageActivity> GetInstagramDirectMessageActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.InstagramDirectMessageSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseInstagramDirectMessageActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<SmsActivity> GetSmsActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.SmsSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseSmsActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<ViberActivity> GetViberActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.ViberSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseViberActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<WhatsAppActivity> GetWhatsAppActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.WhatsAppSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseWhatsAppActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    public IAsyncEnumerable<WebChatActivity> GetWebChatActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => GetListAsync(
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.WebChatSuffix}{IActivityEndpoint.UriPostfix}",
        DaktelaJsonSerializerContext.Default.ListResponseWebChatActivity,
        request,
        requestOption,
        responseBehaviour,
        cancellationToken
    );

    private IAsyncEnumerable<T> GetListAsync<T>(
        string targetUri,
        JsonTypeInfo<ListResponse<T>> jsonTypeInfo,
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) where T : class => pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient,
            httpResponseParser,
            targetUri,
            jsonTypeInfo,
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync(
            ctx.httpResponseParser,
            ctx.targetUri,
            request,
            ctx.jsonTypeInfo,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    #endregion
}
