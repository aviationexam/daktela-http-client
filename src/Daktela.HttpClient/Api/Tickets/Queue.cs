using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class Queue
{
    /// <summary>
    /// Unique queue number
    ///
    /// Unique number of queue
    /// </summary>
    [JsonPropertyName("name")]
    public int Name { get; set; }

    /// <summary>
    /// Title
    ///
    /// Display name
    /// </summary>
    [JsonPropertyName("title")]
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Alias
    ///
    /// The queue's alias is used for example as a visible target for customers using web chat.
    /// </summary>
    [JsonPropertyName("alias")]
    public string Alias { get; set; } = null!;

    /// <summary>
    /// Description
    ///
    /// Optional description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Call steering description
    ///
    /// Enter hint words for the call steering application.
    /// </summary>
    [JsonPropertyName("call_steering_description")]
    public string CallSteeringDescription { get; set; } = null!;

    /// <summary>
    /// Queue type
    ///
    /// Type of queue
    /// </summary>
    [JsonPropertyName("type")]
    public EQueueType Type { get; set; }

    /// <summary>
    /// Direction
    ///
    /// If an activity comes in from the customer or out from the operator
    /// </summary>
    [JsonPropertyName("direction")]
    public EQueueDirection Direction { get; set; }

    /// <summary>
    /// Options
    ///
    /// Additional parameters
    /// </summary>
    [JsonPropertyName("options")]
    public object Options { get; set; } = null!;

    /// <summary>
    /// Deactivated
    ///
    /// Flag if queue is deactivated
    /// </summary>
    [JsonPropertyName("deactivated")]
    public bool Deactivated { get; set; }
}
