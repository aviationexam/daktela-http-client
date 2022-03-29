using Daktela.HttpClient.Api.Tickets;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IActivityEndpoint
{
    protected internal const string UriPrefix = "/api/v6/activities";
    protected internal const string UriPostfix = ".json";

    Task<ReadActivity> CreateActivityAsync(
        CreateActivity ticket, CancellationToken cancellationToken
    );
}
