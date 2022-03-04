using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Requests;
using Moq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Tests.Endpoints;

public static class DaktelaHttpClientMock
{
    public static async Task<IDisposable> MockHttpGetResponse<TContract>(this Mock<IDaktelaHttpClient> mock, string uri, string httpContent)
        where TContract : class
    {
        // disposed from httpResponseContent
        var memoryStream = new MemoryStream();

        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(httpContent);
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var httpResponseContent = new StreamContent(memoryStream);

        mock.Setup(x => x.GetAsync<TContract>(
            It.IsAny<IHttpResponseParser>(),
            uri,
            It.IsAny<CancellationToken>()
        )).Returns((
            IHttpResponseParser httpResponseParser, string _, CancellationToken cancellationToken
        ) => httpResponseParser.ParseResponseAsync<SingleResponse<TContract>>(httpResponseContent, cancellationToken));

        return httpResponseContent;
    }

    public static async Task<IDisposable> MockHttpGetListResponse<TContract>(this Mock<IDaktelaHttpClient> mock, string uri, string httpContent)
        where TContract : class
    {
        // disposed from httpResponseContent
        var memoryStream = new MemoryStream();

        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(httpContent);
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var httpResponseContent = new StreamContent(memoryStream);

        mock.Setup(x => x.GetListAsync<TContract>(
            It.IsAny<IHttpResponseParser>(),
            uri,
            It.IsAny<IRequest>(),
            It.IsAny<CancellationToken>()
        )).Returns((
            IHttpResponseParser httpResponseParser, string _, IRequest _, CancellationToken cancellationToken
        ) => httpResponseParser.ParseResponseAsync<ListResponse<TContract>>(httpResponseContent, cancellationToken));

        return httpResponseContent;
    }
}
