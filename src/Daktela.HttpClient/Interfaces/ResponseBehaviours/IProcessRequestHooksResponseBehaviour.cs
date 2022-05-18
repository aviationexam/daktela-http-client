using Daktela.HttpClient.Api.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.ResponseBehaviours;

public interface IProcessRequestHooksResponseBehaviour : IResponseBehaviour
{
    Task<IAfterPageHookProcessRequestResponseBehaviour> BeforePageAsync(Paging? paging, CancellationToken cancellationToken);
}
