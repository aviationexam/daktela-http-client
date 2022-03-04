using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IContactEndpoint
{
    protected internal const string UriPrefix = "/api/v6/contacts";
    protected internal const string UriPostfix = ".json";

    Task<Contact> GetContactAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<Contact> GetContactsAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseMetadata responseMetadata,
        CancellationToken cancellationToken = default
    );
}
