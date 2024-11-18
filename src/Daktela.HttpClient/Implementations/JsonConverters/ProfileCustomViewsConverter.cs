using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Users;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class ProfileCustomViewsConverter : JsonConverter<ProfileCustomViews>
{
    public override ProfileCustomViews? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    ) => reader.TokenType switch
    {
        JsonTokenType.StartArray => ReadProfileCustomArray(ref reader, typeToConvert, options),
        JsonTokenType.StartObject => ReadProfileCustomDictionary(ref reader, typeToConvert, options),
        JsonTokenType.Null => null,
        _ => throw new JsonException(),
    };

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private ProfileCustomViews ReadProfileCustomArray(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        reader.Read();

        if (reader.TokenType is JsonTokenType.EndArray)
        {
            return new ProfileCustomViews();
        }

        throw new JsonException();
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private ProfileCustomViews ReadProfileCustomDictionary(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        var profileCustomViews = JsonSerializer.Deserialize(
            ref reader,
            DaktelaJsonSerializerContext.Default.IReadOnlyDictionaryStringJsonElement
        );

        return new ProfileCustomViews(profileCustomViews!);
    }

    public override void Write(
        Utf8JsonWriter writer, ProfileCustomViews value, JsonSerializerOptions options
    ) => JsonSerializer.Serialize(writer, value, DaktelaJsonSerializerContext.Default.IReadOnlyDictionaryStringJsonElement);
}
