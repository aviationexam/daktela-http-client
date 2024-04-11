using Daktela.HttpClient.Attributes;
using System.Collections.Generic;
using System.Text.Json;
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
    [JsonPropertyName("title")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Type
    /// </summary>
    [JsonPropertyName("type")]
    public EProfileType Type { get; set; }

    /// <summary>
    /// Max activities
    ///
    /// Maximum allowed number of concurrently incoming activities
    /// </summary>
    [JsonPropertyName("maxActivities")]
    public int? MaxActivities { get; set; }

    /// <summary>
    /// Max outgoing records
    ///
    /// Maximum allowed number of concurrently open outgoing records
    /// </summary>
    [JsonPropertyName("maxOutRecords")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public int? MaxOutRecords { get; set; }

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
    [JsonPropertyName("canTransferCall")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public ECanTransferCall CanTransferCall { get; set; }

    /// <summary>
    /// Call monitoring modes
    ///
    /// Select if and how users with these rights can interact with calls they monitor from the Realtime panel:
    /// Monitor only – can only listen in to calls.
    /// Whisper – can talk to the agent that is handling the call.
    /// Barge – can enter the call and be heard by both the agent and the customer.
    /// Take over – can remove the agent from the call and finish it themselves.
    ///
    /// Whisper, Barge and Take over modes can only be started from the Activities section of the Realtime panel.
    /// </summary>
    [JsonPropertyName("monitoringModes")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public ECallMonitoringMode CallMonitoringModes { get; set; }

    /// <summary>
    /// Daktela Copilot
    ///
    /// Select which Copilot features users with these rights can use.
    /// Summary: Users will be able to summarise Tickets, Contacts, Accounts and Chats.
    /// Compose features: Users will be able to use assisted writing features such Expand, Friendly, Professional, Translate etc.
    /// </summary>
    [JsonPropertyName("gptFunctions")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public EGptFunctions GptFunctions { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonPropertyName("askForAssistance")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public bool AskForAssistance { get; set; }

    /// <summary>
    /// Options
    /// </summary>
    [JsonPropertyName("options")]
    public IReadOnlyDictionary<string, JsonElement> Options { get; set; } = null!;

    /// <summary>
    /// Custom Views
    /// </summary>
    [JsonPropertyName("customViews")]
    public ProfileCustomViews CustomViews { get; set; } = null!;

    /// <summary>
    /// Custom Social Media Views
    /// </summary>
    [JsonPropertyName("customSocialMediaViews")]
    public IReadOnlyDictionary<string, JsonElement> CustomSocialMediaViews { get; set; } = null!;

    /// <summary>
    /// One time password
    ///
    /// When a user's password is changed in Manage → Users, they will be required to change it the next time they log in.
    /// </summary>
    [JsonPropertyName("oneTimePassword")]
    public bool? OneTimePassword { get; set; }

    /// <summary>
    /// Password Reset
    ///
    /// Turn on to allow users to reset their own passwords. They need to have a valid email address filled in the Authentication email field in Manage → Users → List of Users.
    /// </summary>
    [JsonPropertyName("passwordReset")]
    public bool PasswordReset { get; set; }
}
