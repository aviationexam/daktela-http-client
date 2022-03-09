using Daktela.HttpClient.Api.Accounts;
using Daktela.HttpClient.Api.Users;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Contacts;

public class UpdateContact
{
    /// <summary>
    /// Title
    ///
    /// Formatted full name
    /// </summary>
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
    [Required]
    [JsonPropertyName("lastname")]
    public string? LastName { get; set; }

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
    public string? Description { get; set; }

    /// <summary>
    /// Custom fields
    /// </summary>
    [JsonPropertyName("customFields")]
    public CustomFields? CustomFields { get; set; }
}
