using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using System.Text.Json;

namespace Daktela.HttpClient.Implementations;

public class HttpJsonSerializerOptions : IHttpJsonSerializerOptions
{
    public JsonSerializerOptions Value { get; }

    public HttpJsonSerializerOptions(DateTimeOffsetConverter dateTimeOffsetConverter)
    {
        Value = new JsonSerializerOptions();
        Value.Converters.Add(dateTimeOffsetConverter);
        Value.Converters.Add(new TimeSpanConverter());
        Value.Converters.Add(new ReadActivityConverter());
        Value.Converters.Add(new CustomFieldsConverter());
        Value.Converters.Add(new EnumsConverterFactory());
        Value.Converters.Add(new EmailActivityOptionsHeadersAddressConverter());
        Value.Converters.Add(new ErrorResponseConverter());
        Value.Converters.Add(new ErrorFormConverter());
    }
}
