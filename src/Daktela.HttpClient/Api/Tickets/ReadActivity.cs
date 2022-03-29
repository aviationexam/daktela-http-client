using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Users;
using Daktela.HttpClient.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class ReadActivity
{
    /// <summary>
    /// Name
    ///
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Ticket
    /// </summary>
    [JsonPropertyName("ticket")]
    [Required]
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
    [Required]
    public EAction Action { get; set; }

    /// <summary>
    /// Type of the activity
    /// </summary>
    [JsonPropertyName("type")]
    [DaktelaRequirement(EOperation.Create)]
    public EActivityType? Type { get; set; }

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
    public string? Options { get; set; }

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
}
