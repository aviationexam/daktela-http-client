using Daktela.HttpClient.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace Daktela.HttpClient.Implementations;

public class HttpRequestSerializer : IHttpRequestSerializer
{
    private readonly IHttpJsonSerializerOptions _httpJsonSerializerOptions;

    public HttpRequestSerializer(IHttpJsonSerializerOptions httpJsonSerializerOptions)
    {
        _httpJsonSerializerOptions = httpJsonSerializerOptions;
    }

    public HttpContent SerializeRequest<TContract>(
        TContract contract
    ) where TContract : class => JsonContent.Create(
        contract,
        options: _httpJsonSerializerOptions.Value
    );
}
