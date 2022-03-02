using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces;

public interface IHttpResponseParser
{
    Task<T> ParseResponseAsync<T>(
        HttpContent httpResponseContent, CancellationToken cancellationToken
    );
}
