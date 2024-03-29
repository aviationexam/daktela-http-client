using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Endpoints;

public interface IContactEndpoint
{
    protected internal const string UriPrefix = "/api/v6/contacts";
    protected internal const string UriPostfix = ".json";

    Task<ReadContact> GetContactAsync(
        string name,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<ReadContact> GetContactsAsync(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        CancellationToken cancellationToken = default
    );

    Task CreateContactAsync(
        CreateContact contact,
        CancellationToken cancellationToken = default
    );

    Task<ReadContact> UpdateContactAsync(
        string name,
        UpdateContact contact,
        CancellationToken cancellationToken = default
    );

    Task DeleteContactAsync(
        string name, CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<TResult> GetContactsFieldsAsync<TRequest, TResult>(
        TRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        JsonTypeInfo<ListResponse<TResult>> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken = default
    )
        where TRequest : IRequest, IFieldsQuery
        where TResult : class, IFieldResult;
}
