using Daktela.HttpClient.Api.CustomFields;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class CustomFieldsConverter : JsonConverter<ICustomFields>
{
    public override ICustomFields? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => reader.TokenType switch
    {
        JsonTokenType.StartArray => ReadCustomFieldsArray(ref reader, typeToConvert, options),
        JsonTokenType.StartObject => ReadCustomFieldsDictionary(ref reader, typeToConvert, options),
        JsonTokenType.Null => null,
        _ => throw new JsonException(),
    };

    private CustomFields? ReadCustomFieldsArray(
        ref Utf8JsonReader reader,
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var type = typeof(IReadOnlyCollection<string>);
        var innerJsonConverter = (JsonConverter<IReadOnlyCollection<string>>) options.GetTypeInfo(type).Converter;

        var customFields = innerJsonConverter.Read(ref reader, type, options);

        if (customFields is null)
        {
            return null;
        }

        // expect customFields array is empty
        return new CustomFields();
    }

    private CustomFields? ReadCustomFieldsDictionary(
        ref Utf8JsonReader reader,
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var type = typeof(IReadOnlyDictionary<string, IReadOnlyCollection<string>>);

        var innerJsonConverter = (JsonConverter<IReadOnlyDictionary<string, IReadOnlyCollection<string>>>) options.GetTypeInfo(type).Converter;

        var customFields = innerJsonConverter.Read(ref reader, type, options);

        if (customFields is null)
        {
            return null;
        }

        return new CustomFields(customFields);
    }

    public override void Write(
        Utf8JsonWriter writer, ICustomFields value, JsonSerializerOptions options
    )
    {
        if (value is CustomFields customFields)
        {
            var converter = (JsonConverter<CustomFields>) options.GetTypeInfo(typeof(CustomFields)).Converter;

            converter.Write(writer, customFields, options);
        }
        else
        {
            writer.WriteStringValue((string?) null);
        }
    }
}
