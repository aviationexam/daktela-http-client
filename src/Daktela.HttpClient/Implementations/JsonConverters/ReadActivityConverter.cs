using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Tickets;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class ReadActivityConverter : JsonConverter<ReadActivity>
{
    private static readonly string PropertyTypeName = nameof(ReadActivity.Type);
    private static readonly string PropertyItemName = nameof(ReadActivity<object>.Item);

    private static readonly string JsonTypeName = typeof(ReadActivity).GetProperty(PropertyTypeName)!
        .GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? PropertyTypeName;

    private static readonly string JsonItemName = typeof(ReadActivity<>).GetProperty(PropertyItemName)!
        .GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? PropertyItemName;

    public override ReadActivity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var readerClone = reader;

        if (readerClone.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var depth = readerClone.CurrentDepth + 1;
        EActivityType? activityType = null;
        JsonTokenType? itemType = null;
        while (readerClone.Read())
        {
            if (depth < readerClone.CurrentDepth)
            {
                continue;
            }

            switch (readerClone.TokenType)
            {
                case JsonTokenType.PropertyName:
                    if (readerClone.GetString() == JsonTypeName)
                    {
                        readerClone.Read();

                        if (readerClone.TokenType is JsonTokenType.String)
                        {
                            var converter = (JsonConverter<EActivityType>) options.GetTypeInfo(typeof(EActivityType)).Converter;
                            activityType = converter.Read(ref readerClone, typeof(EActivityType), options);
                        }
                    }
                    else if (readerClone.GetString() == JsonItemName)
                    {
                        readerClone.Read();

                        itemType = readerClone.TokenType;
                    }

                    break;

                case JsonTokenType.StartArray:
                case JsonTokenType.StartObject:
                    if (!readerClone.TrySkip())
                    {
                        throw new JsonException($"Unknown error while skipping {readerClone.TokenType}");
                    }

                    break;
            }

            if (itemType.HasValue && activityType.HasValue)
            {
                break;
            }
        }

        if (activityType.HasValue)
        {
            if (itemType == JsonTokenType.Number)
            {
                return JsonSerializer.Deserialize(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityWithNumericReference);
            }

            return activityType switch
            {
                EActivityType.Comment => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityCommentActivity),
                EActivityType.Call => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityCallActivity),
                EActivityType.Email => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityEmailActivity),
                EActivityType.WebChat => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityWebChatActivity),
                EActivityType.Sms => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivitySmsActivity),
                EActivityType.FacebookMessenger => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityFacebookMessengerActivity),
                EActivityType.WhatsApp => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityWhatsAppActivity),
                EActivityType.Viber => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityViberActivity),
                EActivityType.Custom => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityCustomActivity),
                EActivityType.InstagramDirectMessage => Read(ref reader, DaktelaJsonSerializerContext.Default.ReadActivityInstagramDirectMessageActivity),
                _ => throw new ArgumentOutOfRangeException(nameof(activityType), activityType, null),
            };
        }

        throw new JsonException($"Json object {nameof(ReadActivity)} does not contain '{JsonTypeName}' type discriminator");
    }

    private ReadActivity<T>? Read<T>(
        ref Utf8JsonReader reader, JsonTypeInfo<ReadActivity<T>> jsonTypeInfo
    ) where T : class => JsonSerializer.Deserialize(ref reader, jsonTypeInfo);

    public override void Write(
        Utf8JsonWriter writer, ReadActivity value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
