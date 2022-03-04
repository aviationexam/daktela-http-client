using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces;

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
        CancellationToken cancellationToken = default
    );
}
