using Daktela.HttpClient.Api.Files;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IFileEndpoint
{
    protected internal const string UriUpload = "/file/upload.php";
    protected internal const string UriDownload = "/file/download.php";

    Task<TResponse> DownloadFileAsync<TCtx, TResponse>(
        EFileSource fileSource,
        long fileName,
        Func<Stream, TCtx, CancellationToken, Task<TResponse>> handleResponse,
        TCtx ctx,
        CancellationToken cancellationToken = default
    );

    Task<string> UploadFileAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default
    );

    Task<bool> RemoveUploadedFileAsync(
        string fileName,
        CancellationToken cancellationToken = default
    );
}
