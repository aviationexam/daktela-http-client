using Daktela.HttpClient.Api.Files;
using Daktela.HttpClient.Implementations.Endpoints;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.Endpoints;

public class FileEndpointTests
{
    private readonly Mock<IDaktelaHttpClient> _daktelaHttpClientMock = new(MockBehavior.Strict);
    private readonly Mock<IHttpRequestFactory> _httpRequestFactoryMock = new(MockBehavior.Strict);

    private readonly IFileEndpoint _fileEndpoint;

    public FileEndpointTests()
    {
        _fileEndpoint = new FileEndpoint(
            _daktelaHttpClientMock.Object,
            _httpRequestFactoryMock.Object
        );
    }

    [Fact]
    public async Task DownloadFileWorks()
    {
#pragma warning disable CA2000
        var httpRequestMessage = new HttpRequestMessage();
#pragma warning restore CA2000

        var downloadedMessage = new byte[] { 0, 1, 2 };
#pragma warning disable CA2000
        var downloadStream = new MemoryStream(downloadedMessage, false);
#pragma warning restore CA2000

        const int fileId = 42;

        _httpRequestFactoryMock.Setup(x => x.CreateHttpRequestMessage(
                HttpMethod.Post, "/file/download.php", It.Is<ICollection<KeyValuePair<string, string?>>>(i =>
                    i.Count == 4
                    && i.Single(q => q.Key == "mapper").Value == "activitiesComment"
                    && i.Single(q => q.Key == "name").Value == fileId.ToString()
                    && i.Single(q => q.Key == "download").Value == "1"
                    && i.Single(q => q.Key == "fullsize").Value == "1"
                )
            ))
            .Returns(httpRequestMessage)
            .Verifiable();

        _daktelaHttpClientMock.Setup(x => x.RawSendAsync(
                It.Is<HttpRequestMessage>(i =>
                    i == httpRequestMessage
                    && i.Content == null
                ),
                HttpCompletionOption.ResponseHeadersRead,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((HttpRequestMessage innerHttpRequestMessage, HttpCompletionOption _, CancellationToken _) =>
            {
                innerHttpRequestMessage.Dispose();

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(downloadStream),
                };

                return response;
            })
            .Verifiable();

        var cancellationToken = CancellationToken.None;

        var response = await _fileEndpoint.DownloadFileAsync(
            EFileSource.ActivitiesComment,
            fileId,
            static async (fileStream, ctx, cancellationToken) =>
            {
                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream, cancellationToken);

                memoryStream.Seek(0, SeekOrigin.Begin);

                Assert.Equal(ctx.downloadedMessage.Length, memoryStream.Length);
                Assert.True(ctx.downloadedMessage.SequenceEqual(memoryStream.ToArray()));

                return true;
            },
            new
            {
                downloadedMessage,
            },
            cancellationToken
        );

        _httpRequestFactoryMock.Verify();
        _daktelaHttpClientMock.Verify();

        Assert.NotNull(downloadStream);
        Assert.False(downloadStream.CanRead);
        Assert.True(response);
    }

    [Fact]
    public async Task UploadFileWorks()
    {
#pragma warning disable CA2000
        var httpRequestMessage = new HttpRequestMessage();
#pragma warning restore CA2000

#pragma warning disable CA2000
        Stream uploadFileStream = new MemoryStream([0, 1, 2], false);
#pragma warning restore CA2000

        const string uploadFileName = "filename.png";

        const string remoteFileIdentifier = "fileIdentifier";

        _httpRequestFactoryMock.Setup(x => x.CreateHttpRequestMessage(
                HttpMethod.Post, "/file/upload.php", It.Is<ICollection<KeyValuePair<string, string?>>>(i =>
                    i.Count == 1 && i.Single(q => q.Key == "type").Value == "save"
                )
            ))
            .Returns(httpRequestMessage)
            .Verifiable();

        _daktelaHttpClientMock.Setup(x => x.RawSendAsync(
                It.Is<HttpRequestMessage>(i =>
                    i == httpRequestMessage
                    && i.Content is MultipartFormDataContent
                ), It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(static (HttpRequestMessage httpRequestMessage, CancellationToken _) =>
            {
                httpRequestMessage.Dispose();

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(remoteFileIdentifier)
                };

                return response;
            })
            .Verifiable();

        var fileIdentifier = await _fileEndpoint.UploadFileAsync(
            uploadFileStream, uploadFileName, TestContext.Current.CancellationToken
        );

        _httpRequestFactoryMock.Verify();
        _daktelaHttpClientMock.Verify();

        Assert.NotNull(fileIdentifier);
        Assert.Equal(remoteFileIdentifier, fileIdentifier);
    }
}
