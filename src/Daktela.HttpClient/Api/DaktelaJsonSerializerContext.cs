using Daktela.HttpClient.Implementations.JsonConverters;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default
)]
[JsonSerializable(typeof(Contacts.CreateContact))]
[JsonSerializable(typeof(Contacts.UpdateContact))]
[JsonSerializable(typeof(CustomFields.CustomFields))]
[JsonSerializable(typeof(Tickets.CreateActivity))]
[JsonSerializable(typeof(Tickets.CreateTicket))]
[JsonSerializable(typeof(Tickets.UpdateActivity))]
[JsonSerializable(typeof(Tickets.UpdateTicket))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.CallActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.CommentActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.CustomActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.EmailActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.FacebookMessengerActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.InstagramDirectMessageActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.SmsActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.ViberActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.WebChatActivity>))]
[JsonSerializable(typeof(Tickets.ReadActivity<Tickets.Activities.WhatsAppActivity>))]
[JsonSerializable(typeof(Responses.Errors.ComplexErrorResponse))]
[JsonSerializable(typeof(Responses.Errors.ErrorFormMessages))]
[JsonSerializable(typeof(Responses.Errors.NestedErrorForm))]
[JsonSerializable(typeof(Responses.Errors.PlainErrorResponse))]
[JsonSerializable(typeof(Responses.ListResponse<Contacts.ReadContact>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.CallActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.CommentActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.CustomActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.EmailActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.FacebookMessengerActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.InstagramDirectMessageActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.SmsActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.ViberActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.WebChatActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Activities.WhatsAppActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Category>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.ReadActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.ReadActivityAttachment>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.ReadTicket>))]
[JsonSerializable(typeof(Responses.SingleResponse<Contacts.ReadContact>))]
[JsonSerializable(typeof(Responses.SingleResponse<Tickets.ReadTicket>))]
[JsonSerializable(typeof(Responses.SingleResponse<Tickets.ReadActivity>))]
[JsonSerializable(typeof(IDictionary<string, ICollection<string>>))]
public partial class DaktelaJsonSerializerContext : JsonSerializerContext
{
    private static TimeSpan _serializationDateTimeOffset;

    public static TimeSpan SerializationDateTimeOffset
    {
        get => _serializationDateTimeOffset;
        set
        {
            _serializationDateTimeOffset = value;
            _convertersContext = null;
        }
    }

    private static JsonSerializerOptions ConvertersContextOptions => new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        IgnoreReadOnlyFields = false,
        IgnoreReadOnlyProperties = false,
        IncludeFields = false,
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new DateTimeOffsetConverter(SerializationDateTimeOffset),
            new TimeSpanConverter(),
            new ReadActivityConverter(),
            new CustomFieldsConverter(),
            new EnumsConverterFactory(),
            new EmailActivityOptionsHeadersAddressConverter(),
            new ErrorResponseConverter(),
            new ErrorFormConverter(),
        },
    };

    private static DaktelaJsonSerializerContext? _convertersContext;

    /// <summary>
    /// The default <see cref="global::System.Text.Json.Serialization.JsonSerializerContext"/> associated with a default <see cref="global::System.Text.Json.JsonSerializerOptions"/> instance.
    /// </summary>
    public static DaktelaJsonSerializerContext CustomConverters => _convertersContext
        ??= new DaktelaJsonSerializerContext(
            new JsonSerializerOptions(ConvertersContextOptions)
        );
}
