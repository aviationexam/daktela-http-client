using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations.ResponseBehaviours;

internal class EmptyAfterPageHookProcessRequestResponseBehaviour : IAfterPageHookProcessRequestResponseBehaviour
{
    public Task AfterPageAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
