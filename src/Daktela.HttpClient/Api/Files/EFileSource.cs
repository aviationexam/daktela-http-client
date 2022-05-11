using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Files;

public enum EFileSource : byte
{
    /// <summary>
    /// Activities comment
    /// </summary>
    [EnumMember(Value = "activitiesComment")]
    ActivitiesComment = 0,

    /// <summary>
    /// Activities E-mail
    /// </summary>
    [EnumMember(Value = "activitiesEmail")]
    ActivitiesEmail = 1,

    /// <summary>
    /// Activities Web chat
    /// </summary>
    [EnumMember(Value = "activitiesWeb")]
    ActivitiesWeb = 2,

    /// <summary>
    /// Activities Facebook Messenger
    /// </summary>
    [EnumMember(Value = "activitiesFbm")]
    ActivitiesFbm = 3,

    /// <summary>
    /// Activities WhatsApp
    /// </summary>
    [EnumMember(Value = "activitiesWap")]
    ActivitiesWap = 4,

    /// <summary>
    /// Activities Viber
    /// </summary>
    [EnumMember(Value = "activitiesVbr")]
    ActivitiesVbr = 5,
}
