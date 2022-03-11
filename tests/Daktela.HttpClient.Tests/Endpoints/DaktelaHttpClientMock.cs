using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Tests.Infrastructure;
using Moq;
using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Tests.Endpoints;

public static class DaktelaHttpClientMock
{
    public static IDisposable MockHttpGetResponse<TContract>(this Mock<IDaktelaHttpClient> mock, string uri, string resourceName)
        where TContract : class
    {
        var httpResponseContent = new StreamContent(resourceName.LoadEmbeddedJson());

        mock.Setup(x => x.GetAsync<TContract>(
            It.IsAny<IHttpResponseParser>(),
            uri,
            It.IsAny<CancellationToken>()
        )).Returns((
            IHttpResponseParser httpResponseParser, string _, CancellationToken cancellationToken
        ) => httpResponseParser.ParseResponseAsync<SingleResponse<TContract>>(httpResponseContent, cancellationToken));

        return httpResponseContent;
    }

    public static IDisposable MockHttpGetListResponse<TContract>(this Mock<IDaktelaHttpClient> mock, string uri, string resourceName)
        where TContract : class
        => mock.MockHttpGetListResponse<TContract>(uri, _ => true, resourceName);

    public static IDisposable MockHttpGetListResponse<TContract>(
        this Mock<IDaktelaHttpClient> mock, string uri, Expression<Func<IRequest, bool>> requestFilter, string resourceName
    )
        where TContract : class
    {
        var httpResponseContent = new StreamContent(resourceName.LoadEmbeddedJson());

        mock.Setup(x => x.GetListAsync<TContract>(
            It.IsAny<IHttpResponseParser>(),
            uri,
            It.Is(requestFilter),
            It.IsAny<CancellationToken>()
        )).Returns((
            IHttpResponseParser httpResponseParser, string _, IRequest _, CancellationToken cancellationToken
        ) => httpResponseParser.ParseResponseAsync<ListResponse<TContract>>(httpResponseContent, cancellationToken));

        return httpResponseContent;
    }

    public static IDisposable MockHttpPostResponse_BadRequest<TRequest>(
        this Mock<IDaktelaHttpClient> mock, string uri, Expression<Func<TRequest, bool>> requestFilter, string resourceName
    )
        where TRequest : class
    {
        var httpResponseContent = new StreamContent(resourceName.LoadEmbeddedJson());

        mock.Setup(x => x.PostAsync(
            It.IsAny<IHttpRequestSerializer>(),
            It.IsAny<IHttpResponseParser>(),
            uri,
            It.Is(requestFilter),
            It.IsAny<CancellationToken>()
        )).Returns(async (
            IHttpRequestSerializer _,
            IHttpResponseParser httpResponseParser,
            string _, TRequest _,
            CancellationToken cancellationToken
        ) =>
        {
            var response = await httpResponseParser.ParseResponseAsync<SingleResponse<TRequest>>(httpResponseContent, cancellationToken);
            throw new BadRequestException<TRequest>(response.Result, response.Error);
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
