using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Threading;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface ITicketsCategoryEndpoint
{
    protected internal const string UriPrefix = "/api/v6/ticketsCategories";
    protected internal const string UriPostfix = ".json";

    IAsyncEnumerable<Category> GetTicketsCategoriesAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );
}
