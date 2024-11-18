using Daktela.HttpClient.Api.Files;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class FileEndpoint(
    IDaktelaHttpClient daktelaHttpClient,
    IHttpRequestFactory httpRequestFactory
) : IFileEndpoint
{
    private readonly EFileSourceEnumJsonConverter _fileSourceEnumJsonConverter = new();

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

        using var httpRequestMessage = httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Post,
            path,
            [
                KeyValuePair.Create<string, string?>("mapper", mapper),
                KeyValuePair.Create<string, string?>("name", fileName.ToString()),
                KeyValuePair.Create<string, string?>("download", "1"),
                KeyValuePair.Create<string, string?>("fullsize", "1"),
            ]
        );

        using var httpResponse = await daktelaHttpClient.RawSendAsync(
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

        using var httpRequestMessage = httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Post,
            path,
            [
                KeyValuePair.Create<string, string?>("type", "save"),
            ]
        );

        using var content = new MultipartFormDataContent();

        using var fileStreamContent = new StreamContent(fileStream);
        fileStreamContent.Headers.Add("Content-Transfer-Encoding", "binary");

        content.Add(fileStreamContent, "files", fileName);

        httpRequestMessage.Content = content;

        using var httpResponse = await daktelaHttpClient.RawSendAsync(httpRequestMessage, cancellationToken);

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

        using var httpRequestMessage = httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Post,
            path,
            [
                KeyValuePair.Create<string, string?>("type", "remove"),
            ]
        );

        using var content = new MultipartFormDataContent();

#pragma warning disable CA2000
        content.Add(new StringContent("fileNames"), fileName);
#pragma warning restore CA2000

        httpRequestMessage.Content = content;

        using var httpResponse = await daktelaHttpClient.RawSendAsync(httpRequestMessage, cancellationToken);

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
