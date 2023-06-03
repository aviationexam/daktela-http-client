using Daktela.HttpClient.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations;

public class HttpResponseParser : IHttpResponseParser
{
    public async Task<T> ParseResponseAsync<T>(
        HttpContent httpResponseContent,
        JsonTypeInfo<T> jsonTypeInfoForResponseType,
        CancellationToken cancellationToken
    )
    {
        await using var responseStream = await httpResponseContent.ReadAsStreamAsync(cancellationToken);

        var parsedObject = await JsonSerializer.DeserializeAsync(
            responseStream,
            jsonTypeInfoForResponseType,
            cancellationToken
        );

        return parsedObject ?? throw new NullReferenceException(nameof(parsedObject));
    }
}
