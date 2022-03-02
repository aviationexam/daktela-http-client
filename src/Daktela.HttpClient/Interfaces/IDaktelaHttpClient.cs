using System;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces;

public interface IDaktelaHttpClient : IDisposable
{
    Task<T> GetAsync<T>(
        IHttpResponseParser httpResponseParser,
        string uri,
        CancellationToken cancellationToken
    );
}
