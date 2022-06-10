using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.JsonConverters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations;

public class HttpJsonSerializerOptions : IHttpJsonSerializerOptions
{
    public JsonSerializerOptions Value { get; }

    public HttpJsonSerializerOptions(IDateTimeOffsetConverter dateTimeOffsetConverter)
    {
        Value = new JsonSerializerOptions();
        Value.Converters.Add((JsonConverter) dateTimeOffsetConverter);
        Value.Converters.Add(new TimeSpanConverter());
        Value.Converters.Add(new ReadActivityConverter());
        Value.Converters.Add(new CustomFieldsConverter());
        Value.Converters.Add(new EnumsConverterFactory());
        Value.Converters.Add(new EmailActivityOptionsHeadersAddressConverter());
        Value.Converters.Add(new ErrorResponseConverter());
        Value.Converters.Add(new ErrorFormConverter());
    }
}
