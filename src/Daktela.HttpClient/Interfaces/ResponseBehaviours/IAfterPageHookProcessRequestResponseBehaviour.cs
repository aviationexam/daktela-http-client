using System;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces.ResponseBehaviours;

public interface IAfterPageHookProcessRequestResponseBehaviour : IAsyncDisposable
{
    Task AfterPageAsync(CancellationToken cancellationToken);
}
