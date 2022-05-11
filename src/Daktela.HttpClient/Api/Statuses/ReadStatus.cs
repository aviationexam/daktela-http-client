using Daktela.HttpClient.Attributes;
using System;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Statuses;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/statuses">Object</a> represents Status of a ticket.
/// </summary>
public class ReadStatus
{
    /// <summary>
    /// Name
    ///
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Title
    ///
    /// Display name
    /// </summary>
    [JsonPropertyName("title")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Description
    ///
    /// Optional description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Blacklist database
    ///
    /// Define to which blacklist database number with that particular status will be addedBlacklistDatabases
    /// </summary>
    [JsonPropertyName("blacklist_database")]
    public object? BlacklistDatabase { get; set; }

    /// <summary>
    /// Blacklist expiration time
    ///
    /// Relative time that should be added to actual time in order to calculate expiration time for number in blacklist (e.g. +6 days , 19 months)
    /// </summary>
    [JsonPropertyName("blacklist_expiration_time")]
    public DateTimeOffset? BlacklistExpirationTime { get; set; }

    /// <summary>
    /// Validation
    ///
    /// Trigger for validation when saving CC records
    /// </summary>
    [JsonPropertyName("validation")]
    public bool Validation { get; set; }

    /// <summary>
    /// Next call
    ///
    /// Trigger for required nextcall when saving CC records
    /// </summary>
    [JsonPropertyName("nextcall")]
    public bool NextCall { get; set; }

    /// <summary>
    /// Color
    ///
    /// Color of flag on ticket with this status
    /// </summary>
    [JsonPropertyName("color")]
    public string Color { get; set; } = null!;
}
