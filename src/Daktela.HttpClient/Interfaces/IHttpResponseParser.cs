using System.Net.Http;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces;

public interface IHttpResponseParser
{
    Task<T> ParseResponseAsync<T>(
        HttpContent httpResponseContent,
        JsonTypeInfo<T> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    );
}
