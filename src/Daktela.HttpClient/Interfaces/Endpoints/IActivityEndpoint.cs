using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Api.Tickets.Activities;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IActivityEndpoint
{
    protected internal const string UriPrefix = "/api/v6/activities";

    protected internal const string CallSuffix = "Call";
    protected internal const string EmailSuffix = "Email";
    protected internal const string FacebookMessengerSuffix = "Fbm";
    protected internal const string InstagramDirectMessageSuffix = "Igdm";
    protected internal const string SmsSuffix = "Sms";
    protected internal const string ViberSuffix = "Vbr";
    protected internal const string WhatsAppSuffix = "Wap";
    protected internal const string WebChatSuffix = "Web";

    protected internal const string UriPostfix = ".json";

    Task<ReadActivity> GetActivityAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<ReadActivity> GetActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    Task<ReadActivity> CreateActivityAsync(
        CreateActivity ticket,
        CancellationToken cancellationToken = default
    );

    Task<ReadActivity> UpdateActivityAsync(
        string name,
        UpdateActivity ticket,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<TResult> GetActivitiesFieldsAsync<TRequest, TResult>(
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        JsonTypeInfo<ListResponse<TResult>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken = default
    )
        where TRequest : IRequest, IFieldsQuery
        where TResult : class, IFieldResult;

    #region External relations

    IAsyncEnumerable<ReadActivityAttachment> GetActivityAttachmentsAsync(
        string name,
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    #endregion

    #region Activity items

    IAsyncEnumerable<CallActivity> GetCallActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<EmailActivity> GetEmailActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<FacebookMessengerActivity> GetFacebookMessengerActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<InstagramDirectMessageActivity> GetInstagramDirectMessageActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<SmsActivity> GetSmsActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<ViberActivity> GetViberActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<WhatsAppActivity> GetWhatsAppActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<WebChatActivity> GetWebChatActivitiesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    #endregion
}
