using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Daktela.HttpClient.Implementations;

public class HttpJsonSerializerOptions : IHttpJsonSerializerOptions
{
    public JsonSerializerOptions Value { get; }

    public HttpJsonSerializerOptions(IOptions<DaktelaOptions> daktelaOptions)
    {
        Value = new JsonSerializerOptions();
        Value.Converters.Add(new DateTimeOffsetConverter(daktelaOptions));
        Value.Converters.Add(new EnumsConverterFactory());
    }
}
