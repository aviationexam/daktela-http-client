using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
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

    IAsyncEnumerable<IDictionary<string, string>> GetTicketsFieldsAsync<TRequest>(
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    ) where TRequest : IRequest, IFieldsQuery;

    #region External relations

    IAsyncEnumerable<ReadActivity> GetTicketActivitiesAsync(
        long name,
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<IDictionary<string, string>> GetTicketActivitiesFieldsAsync<TRequest>(
        long name,
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    ) where TRequest : IRequest, IFieldsQuery;

    #endregion
}
