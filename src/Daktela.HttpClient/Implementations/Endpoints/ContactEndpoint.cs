using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class ContactEndpoint : IContactEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpRequestSerializer _httpRequestSerializer;
    private readonly IHttpResponseParser _httpResponseParser;
    private readonly IPagedResponseProcessor<IContactEndpoint> _pagedResponseProcessor;

    public ContactEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser,
        IPagedResponseProcessor<IContactEndpoint> pagedResponseProcessor
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpRequestSerializer = httpRequestSerializer;
        _httpResponseParser = httpResponseParser;
        _pagedResponseProcessor = pagedResponseProcessor;
    }

    public async Task<ReadContact> GetContactAsync(
        string name, CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        var contact = await _daktelaHttpClient.GetAsync<ReadContact>(
            _httpResponseParser,
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            cancellationToken
        ).ConfigureAwait(false);

        return contact.Result;
    }

    public IAsyncEnumerable<ReadContact> GetContactsAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => _pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient = _daktelaHttpClient,
            httpResponseParser = _httpResponseParser
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync<ReadContact>(
            ctx.httpResponseParser,
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            request,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    public async Task CreateContactAsync(
        CreateContact contact, CancellationToken cancellationToken
    ) => await _daktelaHttpClient.PostAsync<CreateContact, ReadContact>(
        _httpRequestSerializer,
        _httpResponseParser,
        $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
        contact,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task UpdateContactAsync(
        string name,
        UpdateContact contact,
        CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        await _daktelaHttpClient.PutAsync<UpdateContact, ReadContact>(
            _httpRequestSerializer,
            _httpResponseParser,
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            contact,
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task DeleteContactAsync(
        string name, CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        await _daktelaHttpClient.DeleteAsync(
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            cancellationToken
        ).ConfigureAwait(false);
    }
}
