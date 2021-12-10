using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Api.Tickets
{
    public interface ITicketClient
    {
        Task FetchTicketsAsync(CancellationToken cancellationToken = default);

        Task GetTicketAsync(int ticketId, CancellationToken cancellationToken = default);

        Task CreateTicketAsync(Ticket ticket, CancellationToken cancellationToken = default);

        Task UpdateTicketAsync(int ticketId, Ticket ticket, CancellationToken cancellationToken = default);

        Task DeleteTicketAsync(int ticketId, CancellationToken cancellationToken = default);
    }
}
