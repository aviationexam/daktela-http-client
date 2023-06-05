using Daktela.HttpClient.Implementations.JsonConverters;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Serialization
)]
[JsonSerializable(typeof(Contacts.CreateContact))]
[JsonSerializable(typeof(Contacts.UpdateContact))]
[JsonSerializable(typeof(Tickets.CreateActivity))]
[JsonSerializable(typeof(Tickets.CreateTicket))]
[JsonSerializable(typeof(Tickets.UpdateActivity))]
[JsonSerializable(typeof(Tickets.UpdateTicket))]
[JsonSerializable(typeof(Responses.SingleResponse<Contacts.ReadContact>))]
[JsonSerializable(typeof(Responses.SingleResponse<Tickets.ReadTicket>))]
[JsonSerializable(typeof(Responses.SingleResponse<Tickets.ReadActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Contacts.ReadContact>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.Category>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.ReadActivity>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.ReadActivityAttachment>))]
[JsonSerializable(typeof(Responses.ListResponse<Tickets.ReadTicket>))]
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
