using Daktela.HttpClient.Api.Tickets;
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
        int name,
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
        int name,
        UpdateTicket ticket,
        CancellationToken cancellationToken = default
    );

    Task DeleteTicketAsync(
        int name, CancellationToken cancellationToken = default
    );
}
