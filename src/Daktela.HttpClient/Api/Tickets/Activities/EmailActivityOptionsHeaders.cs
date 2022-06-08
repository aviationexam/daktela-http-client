using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public class EmailActivityOptionsHeaders
{
    [JsonPropertyName("cc")]
    public ICollection<EmailActivityOptionsHeadersAddress>? CarbonCopy { get; set; }

    [JsonPropertyName("bcc")]
    public ICollection<EmailActivityOptionsHeadersAddress>? BlindCarbonCopy { get; set; }

    [JsonPropertyName("to")]
    public ICollection<EmailActivityOptionsHeadersAddress> To { get; set; } = null!;

    [JsonPropertyName("date")]
    public DateTimeOffset? Date { get; set; }

    [JsonPropertyName("from")]
    public ICollection<EmailActivityOptionsHeadersAddress>? From { get; set; }

    [JsonPropertyName("ticket")]
    public string? Ticket { get; set; }

    [JsonPropertyName("reply-to")]
    public string? ReplyTo { get; set; }

    [JsonPropertyName("In-reply-to")]
    public string? InReplyTo { get; set; }

    [JsonPropertyName("message-id")]
    public string? MessageId { get; set; }

    [JsonPropertyName("references")]
    public string References { get; set; } = null!;

    [JsonPropertyName("return-path")]
    public string? ReturnPath { get; set; }

    [JsonPropertyName("auto-submitted")]
    public bool? AutoSubmitted { get; set; }
}
