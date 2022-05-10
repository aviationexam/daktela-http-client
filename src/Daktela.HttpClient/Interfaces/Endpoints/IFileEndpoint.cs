using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IFileEndpoint
{
    protected internal const string UriPrefix = "/file/upload.php";

    Task<string> UploadFileAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken = default
    );

    Task<bool> RemoveFileAsync(
        string fileName,
        CancellationToken cancellationToken = default
    );
}
