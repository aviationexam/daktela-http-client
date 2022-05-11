using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IActivityEndpoint
{
    protected internal const string UriPrefix = "/api/v6/activities";
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

    #region External relations

    IAsyncEnumerable<ReadActivityAttachment> GetActivityAttachmentsAsync(
        string name,
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    );

    #endregion
}
