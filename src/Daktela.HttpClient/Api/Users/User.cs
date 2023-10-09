using Daktela.HttpClient.Api.Integrations;
using Daktela.HttpClient.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Users;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/users">Object Users</a> returns all information about the user - login name, email, accesses, rights and the relationship with other components of the contact center (categories, groups, events, templates, etc.).
/// </summary>
public class User
{
    /// <summary>
    /// Name
    ///
    /// Login name
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
    /// Alias
    ///
    /// User's alias, used for example in webchat as visible nickname for customer
    /// </summary>
    [JsonPropertyName("alias")]
    public string Alias { get; set; } = null!;

    /// <summary>
    /// Role
    ///
    /// User accesses enable you to set rights to specific sections in control panelRoles
    /// </summary>
    [JsonPropertyName("role")]
    public Role Role { get; set; } = null!;

    /// <summary>
    /// Rights
    ///
    /// User rights allows you to add rights to users and queuesProfiles
    /// </summary>
    [JsonPropertyName("profile")]
    public Profile Profile { get; set; } = null!;

    /// <summary>
    /// NPS score
    ///
    /// Calculated NPS score for all activities of this user
    /// </summary>
    [JsonPropertyName("nps_score")]
    public decimal NpsScore { get; set; }

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
    /// Hint words for call steering application
    /// </summary>
    [JsonPropertyName("call_steering_description")]
    public string CallSteeringDescription { get; set; } = null!;

    /// <summary>
    /// Password
    ///
    /// Password
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Extension
    ///
    /// Extension is an internal number that shielding more SIP devices and external numbers
    /// </summary>
    [JsonPropertyName("extension")]
    public string Extension { get; set; } = null!;

    /// <summary>
    /// Custom ACL
    /// </summary>
    [JsonPropertyName("acl")]
    public RightsToCall? Acl { get; set; } = null!;

    /// <summary>
    /// Extension state
    ///
    /// State of physical devices, connected to extension
    /// </summary>
    [JsonPropertyName("extension_state")]
    public EExtensionState? ExtensionState { get; set; }

    /// <summary>
    /// Outgoing identification
    ///
    /// What phone number should the user represent when calling
    /// </summary>
    [JsonPropertyName("clid")]
    public string CallId { get; set; } = null!;

    /// <summary>
    /// Login static
    ///
    /// The type of registered user, static, the user for work should not be authenticated in the control panel
    /// </summary>
    [JsonPropertyName("static")]
    public bool IsStatic { get; set; }

    /// <summary>
    /// Allow call recording interruption
    ///
    /// Defines if the specific user or users calling via the specific queue are able to pause or resume call recording. Note that if this setting differs on the user or the queue, if one of these is set to "Yes", then the call can be interrupted.
    /// </summary>
    [JsonPropertyName("allowRecordingInterruption")]
    public bool AllowRecordingInterruption { get; set; }

    /// <summary>
    /// Record calls
    ///
    /// Defines if and when is the call supposed to be recorded. Note that these settings can be defined on user and queue and if these are set differently, the earlier recording option prevails.  "Do not record": Calls are not recorded. Note that even though this setting is set on user or queue, the other one can have Call recording set to "Yes" and then the call gets recorded.  "Record from call start": Call recording is started from the moment the call first reaches the user or queue (depends on where the setting is defined) which usually includes the ringing and early media sounds.  "Record after call is bridged": Call recording is started when the parties of the call are connected. This usually eliminates useless time of recordings with ringing and early media.
    /// </summary>
    [JsonPropertyName("recordAtCallStart")]
    public ERecordAtCallStart RecordAtCallStart { get; set; }

    /// <summary>
    /// Verify type
    /// </summary>
    [JsonPropertyName("algo")]
    public Config Algo { get; set; } = null!;

    /// <summary>
    /// Notification email
    ///
    /// User email address mainly for notifications
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Authentication email
    ///
    /// Email that is used for Google authentication
    /// </summary>
    [JsonPropertyName("emailAuth")]
    public string EmailAuth { get; set; } = null!;

    /// <summary>
    /// User picture
    ///
    /// User's picture, used for example in webchat as avatar. It is better if picture have a square format.  Allowed file types are: {{onFilesValidation.allowedExtensions | arrayJoin}} Maximal file size is: {{onFilesValidation.maxFileSize | formatSizeUnits}}
    /// </summary>
    [JsonPropertyName("icon")]
    public string Icon { get; set; } = null!;

    /// <summary>
    /// User emoji
    ///
    /// User's emoji. It`s used only in Daktela user interface for better distinction of users.
    /// </summary>
    [JsonPropertyName("emoji")]
    public string Emoji { get; set; } = null!;

    /// <summary>
    /// Options
    ///
    /// Additional parameters
    /// </summary>
    [JsonPropertyName("options")]
    public IReadOnlyDictionary<string, object> Options { get; set; } = null!;

    /// <summary>
    /// Backoffice user
    ///
    /// Flag if user belongs to backoffice category
    /// </summary>
    [JsonPropertyName("backoffice_user")]
    public bool BackofficeUser { get; set; }

    /// <summary>
    /// Forwarding number
    ///
    /// Call forwarding number
    /// </summary>
    [JsonPropertyName("forwarding_number")]
    public string ForwardingNumber { get; set; } = null!;

    /// <summary>
    /// Deactivated
    ///
    /// Deactivated flag
    /// </summary>
    [JsonPropertyName("deactivated")]
    public bool Deactivated { get; set; }

    /// <summary>
    /// Deleted
    ///
    /// Deleted flag
    /// </summary>
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}
