using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations;

public class HttpResponseParser : IHttpResponseParser
{
    private readonly IHttpJsonSerializerOptions _httpJsonSerializerOptions;

    public HttpResponseParser(IHttpJsonSerializerOptions httpJsonSerializerOptions)
    {
        _httpJsonSerializerOptions = httpJsonSerializerOptions;
    }

    public async Task<T> ParseResponseAsync<T>(
        HttpContent httpResponseContent, CancellationToken cancellationToken
    )
    {
        await using var responseStream = await httpResponseContent.ReadAsStreamAsync(cancellationToken);

        var parsedObject = await JsonSerializer.DeserializeAsync<T>(responseStream, _httpJsonSerializerOptions.Value, cancellationToken);

        return parsedObject ?? throw new NullReferenceException(nameof(parsedObject));
    }
}
