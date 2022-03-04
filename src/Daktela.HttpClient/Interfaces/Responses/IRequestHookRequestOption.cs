using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.Responses;

public interface IProcessRequestHooksResponseMetadata : IResponseMetadata
{
    Task BeforePageAsync(Paging? paging, CancellationToken cancellationToken);

    Task AfterPageAsync(CancellationToken cancellationToken);
}
