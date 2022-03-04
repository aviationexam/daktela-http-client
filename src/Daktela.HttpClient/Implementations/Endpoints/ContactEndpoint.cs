using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class ContactEndpoint : IContactEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpResponseParser _httpResponseParser;
    private readonly IPagedResponseProcessor<IContactEndpoint> _pagedResponseProcessor;

    public ContactEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpResponseParser httpResponseParser,
        IPagedResponseProcessor<IContactEndpoint> pagedResponseProcessor
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpResponseParser = httpResponseParser;
        _pagedResponseProcessor = pagedResponseProcessor;
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

    public IAsyncEnumerable<Contact> GetContactsAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseMetadata responseMetadata,
        CancellationToken cancellationToken
    ) => _pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseMetadata,
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
        ) => await ctx.daktelaHttpClient.GetListAsync<Contact>(
            ctx.httpResponseParser,
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            request,
            cancellationToken
        ), cancellationToken
    );
}