using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations;

public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// This method should be used when returning IAsyncEnumerable from library without ConfigureAwait(false)
    /// </summary>
    public static async IAsyncEnumerable<T> IteratingConfigureAwait<T>(
        this IAsyncEnumerable<T> enumerable,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        await foreach (
            var item in enumerable
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false)
        )
        {
            yield return item;
        }
    }
}
