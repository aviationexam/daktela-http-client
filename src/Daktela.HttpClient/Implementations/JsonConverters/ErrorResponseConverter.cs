using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Responses.Errors;
using System;
using System.Diagnostics.CodeAnalysis;
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

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private ComplexErrorResponse? ReadComplexErrorResponse(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonTypeInfo = DaktelaJsonSerializerContext.Default.ComplexErrorResponse;

        return JsonSerializer.Deserialize(ref reader, jsonTypeInfo);
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private PlainErrorResponse? ReadPlainErrorResponse(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonTypeInfo = DaktelaJsonSerializerContext.Default.PlainErrorResponse;

        return JsonSerializer.Deserialize(ref reader, jsonTypeInfo);
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private PlainErrorResponse ReadString(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) =>
    [
        reader.GetString()!
    ];

    public override void Write(
        Utf8JsonWriter writer, IErrorResponse value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
