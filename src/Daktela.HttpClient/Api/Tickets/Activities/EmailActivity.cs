using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Users;
using Daktela.HttpClient.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public class EmailActivity
{
    /// <summary>
    /// Name
    ///
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Queue
    /// </summary>
    [JsonPropertyName("queue")]
    [DaktelaRequirement(EOperation.Read)]
    public Queue Queue { get; set; } = null!;

    /// <summary>
    /// User
    /// </summary>
    [JsonPropertyName("user")]
    public User? User { get; set; }

    /// <summary>
    /// Contact
    /// </summary>
    [JsonPropertyName("contact")]
    public ReadContact? Contact { get; set; }

    /// <summary>
    /// Title
    ///
    /// Subject of email
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Address
    ///
    /// Sender's or recipient's email address
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = null!;

    /// <summary>
    /// Direction
    ///
    /// If activity come in from customer or out from operator
    /// </summary>
    [JsonPropertyName("direction")]
    public EActivityDirection Direction { get; set; }

    /// <summary>
    /// Wait time
    ///
    /// Total waiting time in queue
    /// </summary>
    [JsonPropertyName("wait_time")]
    public TimeSpan? WaitTime { get; set; }

    /// <summary>
    /// Duration
    ///
    /// Time in seconds of creating email
    /// </summary>
    [JsonPropertyName("duration")]
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// Answered
    ///
    /// Mark if email was answered
    /// </summary>
    [JsonPropertyName("answered")]
    public bool Answered { get; set; }

    /// <summary>
    /// Text
    ///
    /// HTML body
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;

    /// <summary>
    /// Options
    ///
    /// Additional email items, like headers
    /// </summary>
    [JsonPropertyName("options")]
    public EmailActivityOptions? Options { get; set; }

    /// <summary>
    /// Time
    ///
    /// Email time
    /// </summary>
    [JsonPropertyName("time")]
    public DateTimeOffset Time { get; set; }

    /// <summary>
    /// Email state
    /// </summary>
    [JsonPropertyName("state")]
    public EEmailActivityStatus? State { get; set; }

    /// <summary>
    /// Activities
    /// </summary>
    [JsonPropertyName("activities")]
    public ICollection<ReadActivity> Activities { get; set; } = null!;
}
