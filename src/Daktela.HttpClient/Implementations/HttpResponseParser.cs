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
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HttpResponseParser(IOptions<DaktelaOptions> daktelaOptions)
    {
        _jsonSerializerOptions = new JsonSerializerOptions();
        _jsonSerializerOptions.Converters.Add(new DateTimeOffsetConverter(daktelaOptions));
        _jsonSerializerOptions.Converters.Add(new EnumsConverterFactory());
    }

    public async Task<T> ParseResponseAsync<T>(
        HttpContent httpResponseContent, CancellationToken cancellationToken
    )
    {
        await using var responseStream = await httpResponseContent.ReadAsStreamAsync(cancellationToken);

        var parsedObject = await JsonSerializer.DeserializeAsync<T>(responseStream, _jsonSerializerOptions, cancellationToken);

        return parsedObject ?? throw new NullReferenceException(nameof(parsedObject));
    }
}
