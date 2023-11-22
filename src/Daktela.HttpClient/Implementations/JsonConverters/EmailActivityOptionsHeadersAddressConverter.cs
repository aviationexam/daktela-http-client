using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Tickets.Activities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class EmailActivityOptionsHeadersAddressConverter : JsonConverter<IReadOnlyCollection<EmailActivityOptionsHeadersAddress>>
{
    public override IReadOnlyCollection<EmailActivityOptionsHeadersAddress>? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => reader.TokenType switch
    {
        JsonTokenType.StartArray => ReadEmailActivityOptionsHeadersAddressArray(ref reader, typeToConvert, options),
        JsonTokenType.String => ReadEmailActivityOptionsHeadersAddressString(ref reader, typeToConvert, options),
        _ => throw new JsonException(),
    };

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private IReadOnlyCollection<EmailActivityOptionsHeadersAddress>? ReadEmailActivityOptionsHeadersAddressString(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        reader.GetString();

        return null;
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private IReadOnlyCollection<EmailActivityOptionsHeadersAddress> ReadEmailActivityOptionsHeadersAddressArray(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        reader.Read();

        var elements = new List<EmailActivityOptionsHeadersAddress>();

        while (reader.TokenType is not JsonTokenType.EndArray)
        {
            elements.Add(JsonSerializer.Deserialize(ref reader, DaktelaJsonSerializerContext.Default.EmailActivityOptionsHeadersAddress)!);

            reader.Read();
        }

        return elements;
    }

    public override void Write(
        Utf8JsonWriter writer, IReadOnlyCollection<EmailActivityOptionsHeadersAddress> value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
