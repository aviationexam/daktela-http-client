using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class ReadTicket
{
    /// <summary>
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    public int Name { get; set; }

    /// <summary>
    /// Subject of ticket
    /// </summary>
    [JsonPropertyName("title")]
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>
    /// A merged ticket Id
    /// </summary>
    [JsonPropertyName("id_merge")]
    public int? MergeId { get; set; }

    /// <summary>
    /// Category
    /// </summary>
    [JsonPropertyName("category")]
    [Required]
    public Category Category { get; set; } = null!;

    /// <summary>
    /// User
    /// </summary>
    [JsonPropertyName("user")]
    public User User { get; set; } = null!;

    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Contact
    /// </summary>
    [JsonPropertyName("contact")]
    public ReadContact? Contact { get; set; }

    /// <summary>
    /// Parent ticket
    /// </summary>
    [JsonPropertyName("parentTicket")]
    public ReadTicket? ParentTicketId { get; set; }

    /// <summary>
    /// Is parent
    ///
    /// Parent flag
    /// </summary>
    [JsonPropertyName("isParent")]
    public bool? IsParent { get; set; }

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
    public EStage Stage { get; set; }

    /// <summary>
    /// Priority
    ///
    /// Level of priority
    /// </summary>
    [JsonPropertyName("priority")]
    public EPriority Priority { get; set; }

    /// <summary>
    /// Sla overdue
    ///
    /// SLA overdue in seconds
    /// </summary>
    [JsonPropertyName("sla_overdue")]
    public int SlaOverdueInSec { get; set; }

    /// <summary>
    /// Deadline
    ///
    /// If ticket is answered (first answer) the close deadline is shown. Otherwise the first answer deadline is shown.
    /// </summary>
    [JsonPropertyName("sla_deadtime")]
    public DateTimeOffset? SlaDeadTime { get; set; }

    /// <summary>
    /// Ticket deadline
    ///
    /// Time till when the ticket needs to be answered
    /// </summary>
    [JsonPropertyName("sla_close_deadline")]
    public DateTimeOffset? SlaCloseDeadline { get; set; }

    /// <summary>
    /// Sla change
    ///
    /// Auxiliary data for sla calculation
    /// </summary>
    [JsonPropertyName("sla_change")]
    public DateTimeOffset? SlaChange { get; set; }

    /// <summary>
    /// Sla duration
    ///
    /// Time in seconds from last change to SLA deadline
    /// </summary>
    [JsonPropertyName("sla_duration")]
    public int SlaDurationInSec { get; set; }

    /// <summary>
    /// Sla custom
    ///
    /// Flag if ticket's deadline was set up manually
    /// </summary>
    [JsonPropertyName("sla_custom")]
    public bool SlaCustom { get; set; }

    /// <summary>
    /// Activity count
    ///
    /// All activities connected to this ticket
    /// </summary>
    [JsonPropertyName("interaction_activity_count")]
    public int InteractionActivityCount { get; set; }

    /// <summary>
    /// Reopen
    ///
    /// Date when the ticket will be automatically re-opened
    /// </summary>
    [JsonPropertyName("reopen")]
    public DateTimeOffset? Reopen { get; set; }

    /// <summary>
    /// Created
    ///
    /// Date of creation
    /// </summary>
    [JsonPropertyName("created")]
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Created by
    /// </summary>
    [JsonPropertyName("created_by")]
    public User CreatedBy { get; set; } = null!;

    /// <summary>
    /// Edited
    ///
    /// Date of last modification
    /// </summary>
    [JsonPropertyName("edited")]
    public DateTimeOffset? Edited { get; set; }

    /// <summary>
    /// Edited by
    /// </summary>
    [JsonPropertyName("edited_by")]
    public User? EditedBy { get; set; }

    /// <summary>
    /// First answer
    ///
    /// Date of first answer
    /// </summary>
    [JsonPropertyName("first_answer")]
    public DateTimeOffset? FirstAnswer { get; set; }

    /// <summary>
    /// First answer duration
    ///
    /// How long does it take until the ticket has been answered
    /// </summary>
    [JsonPropertyName("first_answer_duration")]
    public int? FirstAnswerDuration { get; set; }

    /// <summary>
    /// First answer deadline
    ///
    /// Time till when the ticket needs to be answered
    /// </summary>
    [JsonPropertyName("first_answer_deadline")]
    public DateTimeOffset? FirstAnswerDeadline { get; set; }

    /// <summary>
    /// First answer deadline
    ///
    /// First answer overdue in seconds
    /// </summary>
    [JsonPropertyName("first_answer_overdue")]
    public int? FirstAnswerOverdueInSec { get; set; }

    /// <summary>
    /// Closed
    ///
    /// Date when the ticket was closed
    /// </summary>
    [JsonPropertyName("closed")]
    public DateTimeOffset? Closed { get; set; }

    /// <summary>
    /// Unread
    ///
    /// Flag if the ticket has not been read yet
    /// </summary>
    [JsonPropertyName("unread")]
    public bool Unread { get; set; }

    /// <summary>
    /// Has attachment
    ///
    /// Flag if the ticket has attachment
    /// </summary>
    [JsonPropertyName("has_attachment")]
    public bool? HasAttachment { get; set; }

    /// <summary>
    /// Followers
    ///
    /// -Tickets\Mn_tickets_followers
    /// </summary>
    [JsonPropertyName("followers")]
    public object Followers { get; set; } = null!;

    /// <summary>
    /// Statuses
    ///
    /// -Statuses\Mn_statuses_tickets
    /// </summary>
    [JsonPropertyName("statuses")]
    public object Statuses { get; set; } = null!;

    /// <summary>
    /// Last Activity
    ///
    /// Date and time of last activity of ticket
    /// </summary>
    [JsonPropertyName("last_activity")]
    public int? LastActivity { get; set; }

    /// <summary>
    /// Last Activity of Opeartor
    ///
    /// Date and time of last activity of ticket from operator
    /// </summary>
    [JsonPropertyName("last_activity_operator")]
    public int? LastActivityOperator { get; set; }

    /// <summary>
    /// Last Activity of Client
    ///
    /// Date and time of last activity of ticket from client
    /// </summary>
    [JsonPropertyName("last_activity_client")]
    public int? LastActivityClient { get; set; }

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public ICollection<CustomField>? CustomFields { get; set; }
}
