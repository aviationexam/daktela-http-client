using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Requests;
using Moq;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Daktela.HttpClient.Tests.Endpoints;

public static class DaktelaHttpClientMock
{
    public static IDisposable MockHttpGetResponse<TContract>(this Mock<IDaktelaHttpClient> mock, string uri, string resourceName)
        where TContract : class
    {
        // disposed from httpResponseContent
        var stream = LoadEmbeddedJson(resourceName);

        var httpResponseContent = new StreamContent(stream);

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
        // disposed from httpResponseContent
        var stream = LoadEmbeddedJson(resourceName);

        var httpResponseContent = new StreamContent(stream);

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

    private static Stream LoadEmbeddedJson(string name)
    {
        var resourceName = $"Daktela.HttpClient.Tests.json_responses.{name}.json";
        var assembly = typeof(DaktelaHttpClientMock).GetTypeInfo().Assembly;
        return assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException($"Resource {resourceName} was not found");
    }
}
