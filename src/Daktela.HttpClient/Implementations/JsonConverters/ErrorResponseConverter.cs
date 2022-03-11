using Daktela.HttpClient.Api.Responses.Errors;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class ErrorResponseConverter : JsonConverter<IErrorResponse>
{
    public override IErrorResponse? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => reader.TokenType switch
    {
        JsonTokenType.StartObject => ReadComplexErrorResponse(ref reader, typeToConvert, options),
        JsonTokenType.StartArray => ReadPlainErrorResponse(ref reader, typeToConvert, options),
        JsonTokenType.String => ReadString(ref reader, typeToConvert, options),
        _ => throw new JsonException(),
    };

    private ComplexErrorResponse? ReadComplexErrorResponse(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var type = typeof(ComplexErrorResponse);
        var innerJsonConverter = (JsonConverter<ComplexErrorResponse>) options.GetConverter(type);

        return innerJsonConverter.Read(ref reader, type, options);
    }

    private PlainErrorResponse? ReadPlainErrorResponse(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var type = typeof(PlainErrorResponse);
        var innerJsonConverter = (JsonConverter<PlainErrorResponse>) options.GetConverter(type);

        return innerJsonConverter.Read(ref reader, type, options);
    }

    private PlainErrorResponse ReadString(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => new()
    {
        reader.GetString()!,
    };

    public override void Write(
        Utf8JsonWriter writer, IErrorResponse value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
