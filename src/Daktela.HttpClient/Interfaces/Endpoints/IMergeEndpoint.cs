using Daktela.HttpClient.Api.Accounts;
using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Merge;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Tickets;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IMergeEndpoint
{
    protected internal const string UriPrefix = "/api/v6/merge";
    protected internal const string UriPostfix = ".json";

    Task<SingleResponse<T>> MergeAsync<T>(
        EMergeType type,
        IReadOnlyCollection<string> items,
        JsonTypeInfo<SingleResponse<T>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken = default
    ) where T : class;

    Task<SingleResponse<ReadContact>> MergeContactsAsync(
        IReadOnlyCollection<string> items,
        CancellationToken cancellationToken = default
    );

    Task<SingleResponse<ReadAccount>> MergeAccountsAsync(
        IReadOnlyCollection<string> items,
        CancellationToken cancellationToken = default
    );

    Task<SingleResponse<ReadTicket>> MergeTicketsAsync(
        IReadOnlyCollection<string> items,
        CancellationToken cancellationToken = default
    );
}
