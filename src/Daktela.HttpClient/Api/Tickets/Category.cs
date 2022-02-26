using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class Category
{
    /// <summary>
    /// Name
    ///
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Title
    ///
    /// Subject of category
    /// </summary>
    [JsonPropertyName("title")]
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Sla
    /// </summary>
    [JsonPropertyName("sla")]
    public Sla Sla { get; set; } = null!;

    /// <summary>
    /// Working time
    /// </summary>
    [JsonPropertyName("timecondition")]
    public TimeGroup? TimeCondition { get; set; }

    /// <summary>
    /// Outgoing email queue
    /// </summary>
    [JsonPropertyName("email_queue")]
    public Queue? EmailQueue { get; set; }

    /// <summary>
    /// Outgoing call queue
    /// </summary>
    [JsonPropertyName("call_queue")]
    public Queue? CallQueue { get; set; }

    /// <summary>
    /// Outgoing sms queue
    /// </summary>
    [JsonPropertyName("sms_queue")]
    public Queue? SmsQueue { get; set; }

    /// <summary>
    /// Status required
    ///
    /// Mark if status is required
    /// </summary>
    [JsonPropertyName("status_required")]
    public bool StatusRequired { get; set; }

    /// <summary>
    /// Multiple statuses
    ///
    /// Allow use multiple statuses
    /// </summary>
    [JsonPropertyName("multiple_statuses")]
    public bool MultipleStatuses { get; set; }

    /// <summary>
    /// Close check childs
    ///
    /// Mark if all child tickets must be closed before closing or deleting ticket
    /// </summary>
    [JsonPropertyName("closeCheckChilds")]
    public bool CloseCheckChilds { get; set; }

    /// <summary>
    /// Auto Archive Tickets
    ///
    /// Number of days when ticket will remain closed. After <see cref="AutoArchiveTickets"/> days ticket will be archived automatically
    /// </summary>
    [JsonPropertyName("auto_archive_tickets")]
    public int AutoArchiveTickets { get; set; }
}
