using Daktela.HttpClient.Implementations.JsonConverters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DaktelaContacts = Daktela.HttpClient.Api.Contacts;
using DaktelaCustomFields = Daktela.HttpClient.Api.CustomFields;
using DaktelaResponses = Daktela.HttpClient.Api.Responses;
using DaktelaResponsesErrors = Daktela.HttpClient.Api.Responses.Errors;
using DaktelaTickets = Daktela.HttpClient.Api.Tickets;
using DaktelaTicketsActivities = Daktela.HttpClient.Api.Tickets.Activities;

#if NET8_0_OR_GREATER
using System.Text.Json;
#endif

namespace Daktela.HttpClient.Api;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Default
)]
[JsonSerializable(typeof(DaktelaContacts.CreateContact))]
[JsonSerializable(typeof(DaktelaContacts.UpdateContact))]
[JsonSerializable(typeof(DaktelaCustomFields.CustomFields))]
[JsonSerializable(typeof(DaktelaTickets.CreateActivity))]
[JsonSerializable(typeof(DaktelaTickets.CreateTicket))]
[JsonSerializable(typeof(DaktelaTickets.UpdateActivity))]
[JsonSerializable(typeof(DaktelaTickets.UpdateTicket))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.CallActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.CommentActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.CustomActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.EmailActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.FacebookMessengerActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.InstagramDirectMessageActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.SmsActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.ViberActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.WebChatActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivity<DaktelaTicketsActivities.WhatsAppActivity>))]
[JsonSerializable(typeof(DaktelaTickets.ReadActivityWithNumericReference))]
[JsonSerializable(typeof(DaktelaResponsesErrors.ComplexErrorResponse))]
[JsonSerializable(typeof(DaktelaResponsesErrors.ErrorFormMessages))]
[JsonSerializable(typeof(DaktelaResponsesErrors.NestedErrorForm))]
[JsonSerializable(typeof(DaktelaResponsesErrors.PlainErrorResponse))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaContacts.ReadContact>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.ActivityDirection>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.CallActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.CommentActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.CustomActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.EmailActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.FacebookMessengerActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.InstagramDirectMessageActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.SmsActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.ViberActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.WebChatActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTicketsActivities.WhatsAppActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTickets.Category>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTickets.ReadActivity>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTickets.ReadActivityAttachment>))]
[JsonSerializable(typeof(DaktelaResponses.ListResponse<DaktelaTickets.ReadTicket>))]
[JsonSerializable(typeof(DaktelaResponses.SingleResponse<DaktelaContacts.ReadContact>))]
[JsonSerializable(typeof(DaktelaResponses.SingleResponse<DaktelaTickets.ReadTicket>))]
[JsonSerializable(typeof(DaktelaResponses.SingleResponse<DaktelaTickets.ReadActivity>))]
[JsonSerializable(typeof(IReadOnlyDictionary<string, IReadOnlyCollection<string>>))]
public partial class DaktelaJsonSerializerContext : JsonSerializerContext
{
    private static TimeSpan _serializationDateTimeOffset;

    public static TimeSpan SerializationDateTimeOffset
    {
        get => _serializationDateTimeOffset;
        set
        {
            _serializationDateTimeOffset = value;
            DateTimeOffsetConverter.SetDateTimeOffset(_serializationDateTimeOffset);
        }
    }

    private static readonly DateTimeOffsetConverter DateTimeOffsetConverter = new(SerializationDateTimeOffset);

    static DaktelaJsonSerializerContext()
    {
        SetConverters(s_defaultOptions.Converters);
#if NET8_0_OR_GREATER
        Default = new DaktelaJsonSerializerContext(new JsonSerializerOptions(s_defaultOptions));
#endif
    }

    public static void SetConverters(ICollection<JsonConverter> jsonConverters)
    {
        UseEnumConverters(jsonConverters);
        jsonConverters.Add(DateTimeOffsetConverter);
        jsonConverters.Add(new TimeSpanConverter());
        jsonConverters.Add(new ReadActivityConverter());
        jsonConverters.Add(new CustomFieldsConverter());
        jsonConverters.Add(new EmailActivityOptionsHeadersAddressConverter());
        jsonConverters.Add(new ErrorResponseConverter());
        jsonConverters.Add(new ErrorFormConverter());
    }
}
