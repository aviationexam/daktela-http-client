using Daktela.HttpClient.Api.Responses.Errors;
using System;
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
        JsonTokenType.String => ReadErrorFormMessage(ref reader, typeToConvert, options),
        _ => throw new JsonException(),
    };

    private ErrorFormMessage ReadErrorFormMessage(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => new()
    {
        ErrorMessage = reader.GetString()!,
    };

    private NestedErrorForm? ReadNestedErrorForm(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        //TODO nested form parser
        // Read(ref reader, type, options);

        return new NestedErrorForm();
    }

    public override void Write(
        Utf8JsonWriter writer, IErrorForm value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
