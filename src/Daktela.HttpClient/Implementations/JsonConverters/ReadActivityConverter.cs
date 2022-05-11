using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Api.Tickets.Activities;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class ReadActivityConverter : JsonConverter<ReadActivity>
{
    private static readonly string PropertyName = nameof(ReadActivity.Type);

    private static readonly string JsonTypeName = typeof(ReadActivity).GetProperty(PropertyName)!
        .GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? PropertyName;

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
        while (readerClone.Read())
        {
            if (depth < readerClone.CurrentDepth)
            {
                continue;
            }

            if (
                readerClone.TokenType == JsonTokenType.PropertyName
                && readerClone.GetString() == JsonTypeName
            )
            {
                readerClone.Read();

                var converter = (JsonConverter<EActivityType>) options.GetConverter(typeof(EActivityType));
                var activityType = converter.Read(ref readerClone, typeof(EActivityType), options);

                return activityType switch
                {
                    EActivityType.Comment => Read<CommentActivity>(ref reader, options),
                    EActivityType.Call => Read<CallActivity>(ref reader, options),
                    EActivityType.Email => Read<EmailActivity>(ref reader, options),
                    EActivityType.WebChat => Read<WebChatActivity>(ref reader, options),
                    EActivityType.Sms => Read<SmsActivity>(ref reader, options),
                    EActivityType.Messenger => Read<MessengerActivity>(ref reader, options),
                    EActivityType.WhatsApp => Read<WhatsAppActivity>(ref reader, options),
                    EActivityType.Viber => Read<ViberActivity>(ref reader, options),
                    EActivityType.Custom => Read<CustomActivity>(ref reader, options),
                    _ => throw new ArgumentOutOfRangeException(nameof(activityType), activityType, null),
                };
            }
        }

        throw new JsonException($"Json object {nameof(ReadActivity)} does not contain '{JsonTypeName}' type discriminator");
    }

    private ReadActivity<T>? Read<T>(ref Utf8JsonReader reader, JsonSerializerOptions options)
        where T : class
        => JsonSerializer.Deserialize<ReadActivity<T>>(ref reader, options);

    public override void Write(
        Utf8JsonWriter writer, ReadActivity value, JsonSerializerOptions options
    ) => throw new NotSupportedException();
}
