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

public class ContactEndpoint(
    IDaktelaHttpClient daktelaHttpClient,
    IHttpRequestSerializer httpRequestSerializer,
    IHttpResponseParser httpResponseParser,
    IPagedResponseProcessor<IContactEndpoint> pagedResponseProcessor
) : IContactEndpoint
{
    public async Task<ReadContact> GetContactAsync(
        string name, CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        var contact = await daktelaHttpClient.GetAsync(
            httpResponseParser,
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            DaktelaJsonSerializerContext.Default.SingleResponseReadContact,
            cancellationToken
        ).ConfigureAwait(false);

        return contact.Result;
    }

    public IAsyncEnumerable<ReadContact> GetContactsAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken
    ) => pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient,
            httpResponseParser,
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
            DaktelaJsonSerializerContext.Default.ListResponseReadContact,
            cancellationToken
        ),
        cancellationToken
    ).IteratingConfigureAwait(cancellationToken);

    public async Task CreateContactAsync(
        CreateContact contact, CancellationToken cancellationToken
    ) => await daktelaHttpClient.PostAsync(
        httpRequestSerializer,
        httpResponseParser,
        $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
        contact,
        DaktelaJsonSerializerContext.Default.CreateContact,
        DaktelaJsonSerializerContext.Default.SingleResponseReadContact,
        cancellationToken
    ).ConfigureAwait(false);

    public async Task<ReadContact> UpdateContactAsync(
        string name,
        UpdateContact contact,
        CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        return await daktelaHttpClient.PutAsync(
            httpRequestSerializer,
            httpResponseParser,
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            contact,
            DaktelaJsonSerializerContext.Default.UpdateContact,
            DaktelaJsonSerializerContext.Default.SingleResponseReadContact,
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task DeleteContactAsync(
        string name, CancellationToken cancellationToken
    )
    {
        var encodedName = HttpUtility.UrlEncode(name);

        await daktelaHttpClient.DeleteAsync(
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
        where TResult : class, IFieldResult => pagedResponseProcessor.InvokeAsync(
        request,
        requestOption,
        responseBehaviour,
        new
        {
            daktelaHttpClient,
            httpResponseParser,
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
