using Daktela.HttpClient.Api.CustomFields;
using Daktela.HttpClient.Api.Files;
using Daktela.HttpClient.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class UpdateTicket
{
    /// <summary>
    /// Subject of ticket
    /// </summary>
    [JsonPropertyName("title")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Category
    /// </summary>
    [JsonPropertyName("category")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Category { get; set; } = null!;

    /// <summary>
    /// User
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    /// Contact
    /// </summary>
    [JsonPropertyName("contact")]
    public string? Contact { get; set; }

    /// <summary>
    /// Parent ticket
    /// </summary>
    [JsonPropertyName("parentTicket")]
    public long? ParentTicketId { get; set; }

    /// <summary>
    /// Description
    ///
    /// Optional description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Stage
    ///
    /// OPEN, WAIT, CLOSE
    /// </summary>
    [JsonPropertyName("stage")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public EStage Stage { get; set; }

    /// <summary>
    /// Priority
    ///
    /// Level of priority
    /// </summary>
    [JsonPropertyName("priority")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public EPriority Priority { get; set; }

    /// <summary>
    /// Deadline
    ///
    /// If ticket is answered (first answer) the close deadline is shown. Otherwise the first answer deadline is shown.
    /// </summary>
    [JsonPropertyName("sla_deadtime")]
    public DateTimeOffset? SlaDeadTime { get; set; }

    /// <summary>
    /// Reopen
    ///
    /// Date when the ticket will be automatically re-opened
    /// </summary>
    [JsonPropertyName("reopen")]
    public DateTimeOffset? Reopen { get; set; }

    /// <summary>
    /// Created by
    /// </summary>
    [JsonPropertyName("created_by")]
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Edited by
    /// </summary>
    [JsonPropertyName("edited_by")]
    public string? EditedBy { get; set; }

    /// <summary>
    /// Followers
    /// </summary>
    [JsonPropertyName("followers")]
    public ICollection<string>? Followers { get; set; }

    /// <summary>
    /// Statuses
    /// </summary>
    [JsonPropertyName("statuses")]
    public ICollection<string>? Statuses { get; set; }

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public ICustomFields? CustomFields { get; set; }

    /// <summary>
    /// Comment
    /// </summary>
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    /// <summary>
    /// Comment files
    /// </summary>
    [JsonPropertyName("comment_add_files")]
    public ICollection<CreateFile>? AddFiles { get; set; }
}
