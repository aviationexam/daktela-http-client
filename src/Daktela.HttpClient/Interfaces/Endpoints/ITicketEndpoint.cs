using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface ITicketEndpoint
{
    protected internal const string UriPrefix = "/api/v6/tickets";
    protected internal const string UriPostfix = ".json";

    Task<ReadTicket> GetTicketAsync(
        long name,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<ReadTicket> GetTicketsAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    Task<ReadTicket> CreateTicketAsync(
        CreateTicket ticket, CancellationToken cancellationToken
    );

    Task<ReadTicket> UpdateTicketAsync(
        long name,
        UpdateTicket ticket,
        CancellationToken cancellationToken = default
    );

    Task DeleteTicketAsync(
        long name, CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<TResult> GetTicketsFieldsAsync<TRequest, TResult>(
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        JsonTypeInfo<ListResponse<TResult>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken = default
    )
        where TRequest : IRequest, IFieldsQuery
        where TResult : class, IFieldResult;

    #region External relations

    IAsyncEnumerable<ReadActivity> GetTicketActivitiesAsync(
        long name,
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<TResult> GetTicketActivitiesFieldsAsync<TRequest, TResult>(
        long name,
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        JsonTypeInfo<ListResponse<TResult>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken = default
    )
        where TRequest : IRequest, IFieldsQuery
        where TResult : class, IFieldResult;

    #endregion
}
