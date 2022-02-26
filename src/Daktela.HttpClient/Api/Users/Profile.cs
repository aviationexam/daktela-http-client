using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Users;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/profiles">Object</a>  Profiles returns all information about user profiles - name profile, title and relation objects users, accesses, categories, etc.
/// </summary>
public class Profile
{
    /// <summary>
    /// Name
    ///
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Users count
    /// </summary>
    [JsonPropertyName("usersCount")]
    public string UsersCount { get; set; } = null!;

    /// <summary>
    /// Title
    /// </summary>
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Max activities
    ///
    /// Maximum allowed number of concurrently incoming activities
    /// </summary>
    [JsonPropertyName("maxActivities")]
    public int MaxActivities { get; set; }

    /// <summary>
    /// Max outgoing records
    ///
    /// Maximum allowed number of concurrently open outgoing records
    /// </summary>
    [Required]
    [JsonPropertyName("maxOutRecords")]
    public int MaxOutRecords { get; set; }

    /// <summary>
    /// Can Delete Missed Activity
    /// </summary>
    [JsonPropertyName("deleteMissedActivity")]
    public bool DeleteMissedActivity { get; set; }

    /// <summary>
    /// Can call without a queue
    /// </summary>
    [JsonPropertyName("noQueueCallsAllowed")]
    public bool NoQueueCallsAllowed { get; set; }

    /// <summary>
    /// Can transfer call
    ///
    /// These settings apply only to call transfers from the Daktela GUI or made using the API. Users will be able to transfer calls from SIP devices regardless of the settings here
    /// </summary>
    [Required]
    [JsonPropertyName("canTransferCall")]
    public ECanTransferCall CanTransferCall { get; set; }

    /// <summary>
    /// Options
    /// </summary>
    [JsonPropertyName("options")]
    public IDictionary<string, object> Options { get; set; } = null!;

    /// <summary>
    /// Custom Views
    /// </summary>
    [JsonPropertyName("customViews")]
    public IDictionary<string, object> CustomViews { get; set; } = null!;
}
