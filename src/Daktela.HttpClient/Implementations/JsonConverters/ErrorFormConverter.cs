using Daktela.HttpClient.Api.Responses.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class ErrorFormConverter : JsonConverter<IErrorForm>
{
    public override IErrorForm? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => reader.TokenType switch
    {
        JsonTokenType.StartObject => ReadNestedErrorForm(ref reader, typeToConvert, options),
        JsonTokenType.StartArray => ReadErrorFormMessages(ref reader, typeToConvert, options),
        JsonTokenType.String => ReadErrorFormMessage(ref reader, typeToConvert, options),
        _ => throw new JsonException(),
    };

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private ErrorFormMessage ReadErrorFormMessage(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => new()
    {
        ErrorMessage = reader.GetString()!,
    };

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private ErrorFormMessages? ReadErrorFormMessages(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        var type = typeof(IReadOnlyCollection<string>);
        var innerJsonConverter = (JsonConverter<IReadOnlyCollection<string>>) options.GetConverter(type);

        var errors = innerJsonConverter.Read(ref reader, type, options);

        if (errors is null)
        {
            return null;
        }

        return new ErrorFormMessages
        {
            ErrorMessages = errors,
        };
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private NestedErrorForm? ReadNestedErrorForm(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var type = typeof(IDictionary<string, IErrorForm>);
        var innerJsonConverter = (JsonConverter<IDictionary<string, IErrorForm>>) options.GetConverter(type);

        var errorDictionary = innerJsonConverter.Read(ref reader, type, options);

        if (errorDictionary is null)
        {
            return null;
        }

        return new NestedErrorForm(errorDictionary);
    }

    public override void Write(
        Utf8JsonWriter writer, IErrorForm value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
