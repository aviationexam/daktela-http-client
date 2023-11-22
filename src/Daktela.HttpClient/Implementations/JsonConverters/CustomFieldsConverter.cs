using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.CustomFields;
using System;
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
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        JsonSerializerOptions options
    )
    {
        var customFields = JsonSerializer.Deserialize(
            ref reader,
            DaktelaJsonSerializerContext.Default.IReadOnlyCollectionString
        );

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
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        JsonSerializerOptions options
    )
    {
        var customFields = JsonSerializer.Deserialize(
            ref reader,
            DaktelaJsonSerializerContext.Default.IReadOnlyDictionaryStringIReadOnlyCollectionString
        );

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
            JsonSerializer.Serialize(writer, customFields, DaktelaJsonSerializerContext.Default.CustomFields);
        }
        else
        {
            writer.WriteStringValue((string?) null);
        }
    }
}
