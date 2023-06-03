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
}
