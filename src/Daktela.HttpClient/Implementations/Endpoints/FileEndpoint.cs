using Daktela.HttpClient.Api.Files;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class FileEndpoint : IFileEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpRequestFactory _httpRequestFactory;
    private readonly EFileSourceEnumJsonConverter _fileSourceEnumJsonConverter = new();

    public FileEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpRequestFactory httpRequestFactory
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpRequestFactory = httpRequestFactory;
    }

    public async Task<TResponse> DownloadFileAsync<TCtx, TResponse>(
        EFileSource fileSource,
        long fileName,
        Func<Stream, TCtx, CancellationToken, Task<TResponse>> handleResponse,
        TCtx ctx,
        CancellationToken cancellationToken
    )
    {
        const string path = IFileEndpoint.UriDownload;

        var mapper = Encoding.UTF8.GetString(
            _fileSourceEnumJsonConverter.ToFirstEnumName(fileSource)
        );

        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Post,
            path,
            new NameValueCollection
            {
                ["mapper"] = mapper,
                ["name"] = fileName.ToString(),
                ["download"] = "1",
                ["fullsize"] = "1",
            }
        );

        using var httpResponse = await _daktelaHttpClient.RawSendAsync(
            httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken
        );

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken)
                    .ConfigureAwait(false);

                return await handleResponse(responseStream, ctx, cancellationToken);
            default:
                throw new UnexpectedHttpResponseException(
                    path, httpResponse.StatusCode,
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
        }
    }

    public async Task<string> UploadFileAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken
    )
    {
        const string path = IFileEndpoint.UriUpload;

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
        fileStreamContent.Headers.Add("Content-Transfer-Encoding", "binary");

        content.Add(fileStreamContent, "files", fileName);

        httpRequestMessage.Content = content;

        using var httpResponse = await _daktelaHttpClient.RawSendAsync(httpRequestMessage, cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                var response = await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);

                return response
                    .TrimStart('"')
                    .TrimEnd('"');
            default:
                throw new UnexpectedHttpResponseException(
                    path, httpResponse.StatusCode,
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
        }
    }

    public async Task<bool> RemoveUploadedFileAsync(
        string fileName,
        CancellationToken cancellationToken
    )
    {
        const string path = IFileEndpoint.UriUpload;

        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Post,
            path,
            new NameValueCollection
            {
                ["type"] = "remove",
            }
        );

        using var content = new MultipartFormDataContent();

#pragma warning disable CA2000
        content.Add(new StringContent("fileNames"), fileName);
#pragma warning restore CA2000

        httpRequestMessage.Content = content;

        using var httpResponse = await _daktelaHttpClient.RawSendAsync(httpRequestMessage, cancellationToken);

        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                var response = await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);

                return response == "1";
            default:
                throw new UnexpectedHttpResponseException(
                    path, httpResponse.StatusCode,
                    await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                        .ConfigureAwait(false)
                );
        }
    }
}
