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
[JsonSerializable(typeof(Tickets.ReadActivityWithNumericReference))]
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
            s_defaultContext = null;
            SetConverters();
        }
    }

    static DaktelaJsonSerializerContext()
    {
        SetConverters();
    }

    static void SetConverters()
    {
        s_defaultOptions.Converters.Clear();
        s_defaultOptions.Converters.Add(new DateTimeOffsetConverter(SerializationDateTimeOffset));
        s_defaultOptions.Converters.Add(new TimeSpanConverter());
        s_defaultOptions.Converters.Add(new ReadActivityConverter());
        s_defaultOptions.Converters.Add(new CustomFieldsConverter());
        s_defaultOptions.Converters.Add(new EnumsConverterFactory());
        s_defaultOptions.Converters.Add(new EmailActivityOptionsHeadersAddressConverter());
        s_defaultOptions.Converters.Add(new ErrorResponseConverter());
        s_defaultOptions.Converters.Add(new ErrorFormConverter());
    }
}
