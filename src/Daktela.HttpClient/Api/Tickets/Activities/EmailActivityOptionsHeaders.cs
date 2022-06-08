using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public class EmailActivityOptionsHeaders
{
    [JsonPropertyName("cc")]
    public string CarbonCopy { get; set; } = null!;

    [JsonPropertyName("to")]
    public ICollection<EmailActivityOptionsHeadersAddress> To { get; set; } = null!;

    [JsonPropertyName("date")]
    public DateTimeOffset Date { get; set; }

    [JsonPropertyName("from")]
    public ICollection<EmailActivityOptionsHeadersAddress> From { get; set; } = null!;

    [JsonPropertyName("ticket")]
    public string Ticket { get; set; } = null!;

    [JsonPropertyName("reply-to")]
    public string ReplyTo { get; set; } = null!;

    [JsonPropertyName("message-id")]
    public string MessageId { get; set; } = null!;

    [JsonPropertyName("references")]
    public string References { get; set; } = null!;

    [JsonPropertyName("return-path")]
    public string ReturnPath { get; set; } = null!;

    [JsonPropertyName("auto-submitted")]
    public bool AutoSubmitted { get; set; }
}
