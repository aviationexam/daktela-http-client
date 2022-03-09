using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IContactEndpoint
{
    protected internal const string UriPrefix = "/api/v6/contacts";
    protected internal const string UriPostfix = ".json";

    Task<ReadContact> GetContactAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<ReadContact> GetContactsAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    Task DeleteContactAsync(
        string name, CancellationToken cancellationToken = default
    );
}
