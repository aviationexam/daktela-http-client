using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Interfaces;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Daktela.HttpClient.Implementations;

public class ContactEndpoint : IContactEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpResponseParser _httpResponseParser;

    public ContactEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpResponseParser httpResponseParser
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpResponseParser = httpResponseParser;
    }

    public async Task<Contact> GetContactAsync(
        string name, CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        var contact = await _daktelaHttpClient.GetAsync<Contact>(
            _httpResponseParser,
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            cancellationToken
        );

        return contact.Result;
    }

    public async IAsyncEnumerable<Contact> GetContactsAsync(
        IRequest request, [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        var contacts = await _daktelaHttpClient.GetListAsync<Contact>(
            _httpResponseParser,
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            request,
            cancellationToken
        );

        yield break;
    }
}
