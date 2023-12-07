using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public class EmailActivityOptionsHeaders
{
    [JsonPropertyName("cc")]
    public IReadOnlyCollection<EmailActivityOptionsHeadersAddress>? CarbonCopy { get; set; }

    [JsonPropertyName("bcc")]
    public IReadOnlyCollection<EmailActivityOptionsHeadersAddress>? BlindCarbonCopy { get; set; }

    [JsonPropertyName("to")]
    public IReadOnlyCollection<EmailActivityOptionsHeadersAddress> To { get; set; } = null!;

    [JsonPropertyName("date")]
    public DateTimeOffset? Date { get; set; }

    [JsonPropertyName("from")]
    public IReadOnlyCollection<EmailActivityOptionsHeadersAddress>? From { get; set; }

    [JsonPropertyName("ticket")]
    public string? Ticket { get; set; }

    [JsonPropertyName("reply-to")]
    public IReadOnlyCollection<EmailActivityOptionsHeadersAddress> ReplyTo { get; set; } = null!;

    [JsonPropertyName("In-reply-to")]
    public string? InReplyTo { get; set; }

    [JsonPropertyName("message-id")]
    public string? MessageId { get; set; }

    /// <summary>
    /// In early stages of activity this field may be empty
    /// </summary>
    [JsonPropertyName("references")]
    public string? References { get; set; }

    [JsonPropertyName("return-path")]
    public string? ReturnPath { get; set; }

    [JsonPropertyName("auto-submitted")]
    public bool? AutoSubmitted { get; set; }
}
