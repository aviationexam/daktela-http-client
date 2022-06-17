using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.CustomFields;
using Daktela.HttpClient.Api.Users;
using Daktela.HttpClient.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/activities">Object</a> represents Activity of a Ticket. One activity have single ticket, one ticket can have many activities.
/// </summary>
public class ReadActivity
{
    /// <summary>
    /// Name
    ///
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Ticket
    /// </summary>
    [JsonPropertyName("ticket")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public ReadTicket Ticket { get; set; } = null!;

    /// <summary>
    /// Title
    ///
    /// Display name
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Important
    /// </summary>
    [JsonPropertyName("important")]
    public bool? Important { get; set; }

    /// <summary>
    /// Actual state of the activity
    /// </summary>
    [JsonPropertyName("action")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public EAction Action { get; set; }

    /// <summary>
    /// Type of the activity
    /// </summary>
    [JsonPropertyName("type")]
    [DaktelaRequirement(EOperation.Create)]
    public EActivityType Type { get; set; }

    /// <summary>
    /// Queue
    /// </summary>
    [JsonPropertyName("queue")]
    public Queue? CallQueue { get; set; }

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
    /// NPS survey
    /// </summary>
    [JsonPropertyName("survey")]
    public object? Survey { get; set; }

    /// <summary>
    /// Campaign record
    /// </summary>
    [JsonPropertyName("record")]
    public object? Record { get; set; }

    /// <summary>
    /// Priority
    /// </summary>
    [JsonPropertyName("priority")]
    public int Priority { get; set; }

    /// <summary>
    /// Additional parameters
    ///
    /// Json - Undefined fields
    /// </summary>
    [JsonPropertyName("options")]
    public object? Options { get; set; }

    /// <summary>
    /// Optional description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Time of creation
    /// </summary>
    [JsonPropertyName("time")]
    public DateTimeOffset? Time { get; set; }

    /// <summary>
    /// Waiting time
    /// </summary>
    [JsonPropertyName("time_wait")]
    public DateTimeOffset? TimeWait { get; set; }

    /// <summary>
    /// Open time
    /// </summary>
    [JsonPropertyName("time_open")]
    public DateTimeOffset? TimeOpen { get; set; }

    /// <summary>
    /// Closed time
    /// </summary>
    [JsonPropertyName("time_close")]
    public DateTimeOffset? TimeClose { get; set; }

    /// <summary>
    /// Before activity work time
    ///
    /// Time sec
    /// </summary>
    [JsonPropertyName("baw")]
    public TimeSpan BeforeActivityWork { get; set; }

    /// <summary>
    /// After activity work time
    ///
    /// Time sec
    /// </summary>
    [JsonPropertyName("aaw")]
    public TimeSpan AfterActivityWork { get; set; }

    /// <summary>
    /// Duration of activity
    ///
    /// Time sec
    /// </summary>
    [JsonPropertyName("duration")]
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Duration of activity's ringing
    ///
    /// Time sec
    /// </summary>
    [JsonPropertyName("ringing_time")]
    public TimeSpan RingingTime { get; set; }

    /// <summary>
    /// Created by user
    /// </summary>
    [JsonPropertyName("created_by")]
    public User? CreatedBy { get; set; }

    /// <summary>
    /// Statuses
    ///
    /// -Statuses\Mn_statuses_tickets
    /// </summary>
    [JsonPropertyName("statuses")]
    public object? Statuses { get; set; }

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public ICustomFields? CustomFields { get; set; }

    /// <summary>
    /// Attachment
    /// </summary>
    [JsonPropertyName("attachments")]
    public ICollection<ReadActivityAttachment>? Attachments { get; set; }
}

public class ReadActivity<TActivity> : ReadActivity
    where TActivity : class
{
    /// <summary>
    /// Specific item of the activity (e.g. Call, Email, Chat,..)
    /// </summary>
    [JsonPropertyName("item")]
    public TActivity? Item { get; set; }
}
