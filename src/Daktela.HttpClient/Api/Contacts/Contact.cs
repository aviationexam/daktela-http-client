using Daktela.HttpClient.Api.Accounts;
using Daktela.HttpClient.Api.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Contacts;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/contacts">Contacts</a> represents the person from your address book or CRM.
/// </summary>
public class Contact
{
    /// <summary>
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Title
    ///
    /// Formated full name
    /// </summary>
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// First name
    /// </summary>
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name
    /// </summary>
    [Required]
    [JsonPropertyName("lastname")]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Account
    /// </summary>
    [JsonPropertyName("account")]
    public Account? Account { get; set; }

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
    /// NPS score
    ///
    /// Calculated NPS score for all activities of this contact
    /// </summary>
    [JsonPropertyName("nps_score")]
    public decimal NpsScore { get; set; }

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
    /// Duplicate contacts
    /// </summary>
    [JsonPropertyName("duplicateContacts")]
    public object? DuplicateContacts { get; set; }

    /// <summary>
    /// Not duplicate contacts
    /// </summary>
    [JsonPropertyName("notDuplicateContacts")]
    public object? NotDuplicateContacts { get; set; }

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public CustomFields? CustomFields { get; set; }
}
