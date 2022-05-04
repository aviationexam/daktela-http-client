using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Api.Users;
using Daktela.HttpClient.Attributes;
using System;
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
    [JsonPropertyName("title")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
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
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Edited
    ///
    /// Date of last modification
    /// </summary>
    [JsonPropertyName("edited")]
    public DateTimeOffset Edited { get; set; }

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public CustomFields CustomFields { get; set; } = null!;
}
