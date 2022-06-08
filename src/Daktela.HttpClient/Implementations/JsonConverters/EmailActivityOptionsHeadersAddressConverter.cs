using Daktela.HttpClient.Api.Tickets.Activities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class EmailActivityOptionsHeadersAddressConverter : JsonConverter<ICollection<EmailActivityOptionsHeadersAddress>>
{
    public override ICollection<EmailActivityOptionsHeadersAddress>? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => reader.TokenType switch
    {
        JsonTokenType.StartArray => ReadEmailActivityOptionsHeadersAddressArray(ref reader, typeToConvert, options),
        JsonTokenType.String => ReadEmailActivityOptionsHeadersAddressString(ref reader, typeToConvert, options),
        _ => throw new JsonException(),
    };

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private ICollection<EmailActivityOptionsHeadersAddress>? ReadEmailActivityOptionsHeadersAddressString(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        reader.GetString();

        return null;
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private ICollection<EmailActivityOptionsHeadersAddress>? ReadEmailActivityOptionsHeadersAddressArray(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        var type = typeof(EmailActivityOptionsHeadersAddress);
        var innerJsonConverter = (JsonConverter<EmailActivityOptionsHeadersAddress>) options.GetConverter(type);

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }
        reader.Read();

        var elements = new List<EmailActivityOptionsHeadersAddress>();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            elements.Add(innerJsonConverter.Read(ref reader, type, options)!);

            reader.Read();
        }

        return elements;
    }

    public override void Write(
        Utf8JsonWriter writer, ICollection<EmailActivityOptionsHeadersAddress> value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
