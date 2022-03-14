using Daktela.HttpClient.Attributes;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Contacts;

public class UpdateContact
{
    /// <summary>
    /// Title
    ///
    /// Formatted full name
    /// </summary>
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    [JsonPropertyName("firstname")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    [JsonPropertyName("lastname")]
    public string? LastName { get; set; }

    /// <summary>
    /// Account
    /// </summary>
    [JsonPropertyName("account")]
    public string? Account { get; set; }

    /// <summary>
    /// User
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    /// Description
    ///
    /// Optional description
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public CustomFields? CustomFields { get; set; }
}
