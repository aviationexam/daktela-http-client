using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Api.Tickets.Activities;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        readerClone.Read();

        var depth = readerClone.CurrentDepth;
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
                            var converter = (JsonConverter<EActivityType>) options.GetConverter(typeof(EActivityType));
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
                    readerClone.Skip();

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
                return JsonSerializer.Deserialize<ReadActivityWithNumericReference>(ref reader, options);
            }

            return activityType switch
            {
                EActivityType.Comment => Read<CommentActivity>(ref reader, options),
                EActivityType.Call => Read<CallActivity>(ref reader, options),
                EActivityType.Email => Read<EmailActivity>(ref reader, options),
                EActivityType.WebChat => Read<WebChatActivity>(ref reader, options),
                EActivityType.Sms => Read<SmsActivity>(ref reader, options),
                EActivityType.FacebookMessenger => Read<FacebookMessengerActivity>(ref reader, options),
                EActivityType.WhatsApp => Read<WhatsAppActivity>(ref reader, options),
                EActivityType.Viber => Read<ViberActivity>(ref reader, options),
                EActivityType.Custom => Read<CustomActivity>(ref reader, options),
                EActivityType.InstagramDirectMessage => Read<InstagramDirectMessageActivity>(ref reader, options),
                _ => throw new ArgumentOutOfRangeException(nameof(activityType), activityType, null),
            };
        }

        throw new JsonException($"Json object {nameof(ReadActivity)} does not contain '{JsonTypeName}' type discriminator");
    }

    private ReadActivity<T>? Read<T>(
        ref Utf8JsonReader reader, JsonSerializerOptions options
    ) where T : class => JsonSerializer.Deserialize<ReadActivity<T>>(ref reader, options);

    public override void Write(
        Utf8JsonWriter writer, ReadActivity value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
