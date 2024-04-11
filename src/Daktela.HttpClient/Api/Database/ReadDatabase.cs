using Daktela.HttpClient.Attributes;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Database;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/crmdatabases">Object</a> represents CRM Databases. Contacts represents the person from your address book or CRM.
/// </summary>
public class ReadDatabase
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
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Available for Cloud Phone users
    /// </summary>
    [JsonPropertyName("available_cp")]
    public bool AvailableCloudPhone { get; set; }

    /// <summary>
    /// Autofocused contacts tab
    /// </summary>
    [JsonPropertyName("autofocused_contacts_tab")]
    public string? AutofocusedContactsTab { get; set; }

    /// <summary>
    /// Autofocused accounts tab
    /// </summary>
    [JsonPropertyName("autofocused_accounts_tab")]
    public string? AutofocusedAccountsTab { get; set; }

    /// <summary>
    /// Default
    /// </summary>
    [JsonPropertyName("default")]
    public bool? Default { get; set; }

    /// <summary>
    /// Reverse Name and Surname
    /// </summary>
    [JsonPropertyName("reverse_names")]
    public bool? ReverseNames { get; set; }
}
