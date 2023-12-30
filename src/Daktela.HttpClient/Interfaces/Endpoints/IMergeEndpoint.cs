using Daktela.HttpClient.Api.Merge;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IMergeEndpoint
{
    protected internal const string UriPrefix = "/api/v6/merge";
    protected internal const string UriPostfix = ".json";

    Task<bool> MergeAsync(
        EMergeType type,
        IReadOnlyCollection<string> items,
        CancellationToken cancellationToken = default
    );
}
