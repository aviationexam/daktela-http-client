using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Api.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Accounts;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/accounts">Object</a> represents CRM Account/Company of Contact. One account can have more contacts, one contact can have only one account.
/// </summary>
public class Account
{
    /// <summary>
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Title
    ///
    /// Display name
    /// </summary>
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Sla
    /// </summary>
    [JsonPropertyName("sla")]
    public Sla Sla { get; set; } = null!;

    /// <summary>
    /// User
    /// </summary>
    [JsonPropertyName("user")]
    public User? User { get; set; }

    /// <summary>
    /// Description
    ///
    /// Optional description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Created
    ///
    /// Date of creation
    /// </summary>
    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    /// <summary>
    /// Edited
    ///
    /// Date of last modification
    /// </summary>
    [JsonPropertyName("edited")]
    public DateTime Edited { get; set; }

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public CustomFields CustomFields { get; set; } = null!;
}
