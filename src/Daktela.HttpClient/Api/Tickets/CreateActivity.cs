using Daktela.HttpClient.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class CreateActivity
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
    public int Ticket { get; set; }

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
    public string? User { get; set; }

    /// <summary>
    /// Contact
    /// </summary>
    [JsonPropertyName("contact")]
    public string? Contact { get; set; }

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
    /// Statuses
    ///
    /// -Statuses\Mn_statuses_tickets
    /// </summary>
    [JsonPropertyName("statuses")]
    public object Statuses { get; set; } = null!;
}
