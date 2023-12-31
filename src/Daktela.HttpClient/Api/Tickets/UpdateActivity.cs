using Daktela.HttpClient.Api.CustomFields;
using Daktela.HttpClient.Api.Files;
using Daktela.HttpClient.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class UpdateActivity
{
    /// <summary>
    /// Ticket
    /// </summary>
    [JsonPropertyName("ticket")]
    [DaktelaNonZeroValue(EOperation.Create | EOperation.Update)]
    public long Ticket { get; set; }

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
    /// Queue
    /// </summary>
    [JsonPropertyName("queue")]
    public Queue? CallQueue { get; set; }

    /// <summary>
    /// Contact
    /// </summary>
    [JsonPropertyName("contact")]
    public string? Contact { get; set; }

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
    /// Statuses
    ///
    /// -Statuses\Mn_statuses_tickets
    /// </summary>
    [JsonPropertyName("statuses")]
    public IReadOnlyCollection<string> Statuses { get; set; } = null!;

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public ICustomFields? CustomFields { get; set; }

    /// <summary>
    /// Files
    /// </summary>
    [JsonPropertyName("add_files")]
    public IReadOnlyCollection<CreateFile>? AddFiles { get; set; }

    /// <summary>
    /// Comment files
    /// </summary>
    [JsonPropertyName("comment_add_files")]
    public IReadOnlyCollection<CreateFile>? AddCommentFiles { get; set; }
}
