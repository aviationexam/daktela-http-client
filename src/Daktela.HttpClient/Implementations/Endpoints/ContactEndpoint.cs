using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
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

        var contact = await _daktelaHttpClient.GetAsync(
            _httpResponseParser,
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            DaktelaJsonSerializerContext.CustomConverters.SingleResponseReadContact,
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
        ) => await ctx.daktelaHttpClient.GetListAsync(
            ctx.httpResponseParser,
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            request,
            DaktelaJsonSerializerContext.CustomConverters.ListResponseReadContact,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    public async Task CreateContactAsync(
        CreateContact contact, CancellationToken cancellationToken
    ) => await _daktelaHttpClient.PostAsync(
        _httpRequestSerializer,
        _httpResponseParser,
        $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
        contact,
        DaktelaJsonSerializerContext.CustomConverters.CreateContact,
        DaktelaJsonSerializerContext.CustomConverters.SingleResponseReadContact,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task<ReadContact> UpdateContactAsync(
        string name,
        UpdateContact contact,
        CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        return await _daktelaHttpClient.PutAsync(
            _httpRequestSerializer,
            _httpResponseParser,
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            contact,
            DaktelaJsonSerializerContext.CustomConverters.UpdateContact,
            DaktelaJsonSerializerContext.CustomConverters.SingleResponseReadContact,
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

    public IAsyncEnumerable<TResult> GetContactsFieldsAsync<TRequest, TResult>(
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        JsonTypeInfo<ListResponse<TResult>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
        where TRequest : IRequest, IFieldsQuery
        where TResult : class, IFieldResult => _pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient = _daktelaHttpClient,
            httpResponseParser = _httpResponseParser,
            jsonTypeInfoForResponseType,
        },
        async static (
            request,
            _,
            _,
            ctx,
            cancellationToken
        ) => await ctx.daktelaHttpClient.GetListAsync(
            ctx.httpResponseParser,
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            request,
            ctx.jsonTypeInfoForResponseType,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);
}
