using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class FileEndpoint : IFileEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpRequestFactory _httpRequestFactory;

    public FileEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpRequestFactory httpRequestFactory
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpRequestFactory = httpRequestFactory;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string name, string fileName, CancellationToken cancellationToken)
    {
        const string path = IFileEndpoint.UriPrefix;

        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Post,
            path,
            new NameValueCollection
            {
                ["type"] = "save",
            }
        );

        using var content = new MultipartFormDataContent();
        using var fileStreamContent = new StreamContent(fileStream);
        content.Add(fileStreamContent, name, fileName);

        httpRequestMessage.Content = content;

        using var httpResponse = await _daktelaHttpClient.RawSendAsync(httpRequestMessage, cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                var response = await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);

                return response;
            default:
                throw new UnexpectedHttpResponseException(
                    path, httpResponse.StatusCode,
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
        }
    }
}
