using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Tests.Infrastructure;
using Moq;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Tests.Endpoints;

public static class DaktelaHttpClientMock
{
    public static IDisposable MockHttpGetResponse<TContract>(this Mock<IDaktelaHttpClient> mock, string uri,
        string resourceName)
        where TContract : class
    {
        var httpResponseContent = new StreamContent(resourceName.LoadEmbeddedJson());

        mock.Setup(x => x.GetAsync(
            It.IsAny<IHttpResponseParser>(),
            uri,
            It.IsAny<JsonTypeInfo<SingleResponse<TContract>>>(),
            It.IsAny<CancellationToken>()
        )).Returns((
            IHttpResponseParser httpResponseParser, string _,
            JsonTypeInfo<SingleResponse<TContract>> jsonTypeInfoForResponseType,
            CancellationToken cancellationToken
        ) => httpResponseParser.ParseResponseAsync(
            httpResponseContent,
            jsonTypeInfoForResponseType,
            cancellationToken
        ));

        return httpResponseContent;
    }

    public static IDisposable MockHttpGetListResponse<TContract>(this Mock<IDaktelaHttpClient> mock, string uri,
        string resourceName)
        where TContract : class
        => mock.MockHttpGetListResponse<TContract>(uri, _ => true, resourceName);

    public static IDisposable MockHttpGetListResponse<TContract>(
        this Mock<IDaktelaHttpClient> mock, string uri, Expression<Func<IRequest, bool>> requestFilter,
        string resourceName
    )
        where TContract : class
    {
        var httpResponseContent = new StreamContent(resourceName.LoadEmbeddedJson());

        mock.Setup(x => x.GetListAsync(
            It.IsAny<IHttpResponseParser>(),
            uri,
            It.Is(requestFilter),
            It.IsAny<JsonTypeInfo<ListResponse<TContract>>>(),
            It.IsAny<CancellationToken>()
        )).Returns((
            IHttpResponseParser httpResponseParser, string _, IRequest _,
            JsonTypeInfo<ListResponse<TContract>> jsonTypeInfoForResponseType,
            CancellationToken cancellationToken
        ) => httpResponseParser.ParseResponseAsync(
            httpResponseContent,
            jsonTypeInfoForResponseType,
            cancellationToken
        ));

        return httpResponseContent;
    }

    public static IDisposable MockHttpPostResponse_BadRequest<TRequest, TResponseContract>(
        this Mock<IDaktelaHttpClient> mock, string uri, Expression<Func<TRequest, bool>> requestFilter,
        string resourceName
    )
        where TRequest : class
        where TResponseContract : class
    {
        var httpResponseContent = new StreamContent(resourceName.LoadEmbeddedJson());

        mock.Setup(x => x.PostAsync(
            It.IsAny<IHttpRequestSerializer>(),
            It.IsAny<IHttpResponseParser>(),
            uri,
            It.Is(requestFilter),
            It.IsAny<JsonTypeInfo<TRequest>>(),
            It.IsAny<JsonTypeInfo<SingleResponse<TResponseContract>>>(),
            It.IsAny<CancellationToken>()
        )).Returns(async (
            IHttpRequestSerializer _,
            IHttpResponseParser httpResponseParser,
            string _, TRequest _,
            JsonTypeInfo<TRequest> _,
            JsonTypeInfo<SingleResponse<TResponseContract>> jsonTypeInfoForResponseType,
            CancellationToken cancellationToken
        ) =>
        {
            var response = await httpResponseParser.ParseResponseAsync(
                httpResponseContent,
                jsonTypeInfoForResponseType,
                cancellationToken
            );

            throw new BadRequestException<TResponseContract>(response.Result, response.Error);
        });

        return httpResponseContent;
    }

    public static void MockHttpDeleteResponse(this Mock<IDaktelaHttpClient> mock, string uri)
    {
        mock.Setup(x => x.DeleteAsync(
            uri,
            It.IsAny<CancellationToken>()
        )).Returns(Task.CompletedTask);
    }
}
