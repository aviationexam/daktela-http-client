using System.Net.Http;
using System.Text.Json.Serialization.Metadata;

namespace Daktela.HttpClient.Interfaces;

public interface IHttpRequestSerializer
{
    HttpContent SerializeRequest<TContract>(
        TContract contract,
        JsonTypeInfo<TContract> jsonTypeInfoForRequestType
    ) where TContract : class;
}
