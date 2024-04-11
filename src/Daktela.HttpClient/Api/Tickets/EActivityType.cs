using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

[EnumJsonConverter]
public enum EActivityType : byte
{
    /// <summary>
    /// Comment
    /// </summary>
    [EnumMember(Value = "")]
    Comment = 0,

    /// <summary>
    /// Call
    /// </summary>
    [EnumMember(Value = "CALL")]
    Call = 1,

    /// <summary>
    /// Email
    /// </summary>
    [EnumMember(Value = "EMAIL")]
    Email = 2,

    /// <summary>
    /// Web chat
    /// </summary>
    [EnumMember(Value = "CHAT")]
    WebChat = 3,

    /// <summary>
    /// SMS
    /// </summary>
    [EnumMember(Value = "SMS")]
    Sms = 4,

    /// <summary>
    /// Facebook messenger
    /// </summary>
    [EnumMember(Value = "FBM")]
    FacebookMessenger = 5,

    /// <summary>
    /// WhatsApp
    /// </summary>
    [EnumMember(Value = "WAP")]
    WhatsApp = 6,

    /// <summary>
    /// Viber
    /// </summary>
    [EnumMember(Value = "VBR")]
    Viber = 7,

    /// <summary>
    /// Custom
    /// </summary>
    [EnumMember(Value = "CUSTOM")]
    Custom = 8,

    /// <summary>
    /// Instagram DM
    /// </summary>
    [EnumMember(Value = "IGDM")]
    InstagramDirectMessage = 9,

    /// <summary>
    /// FB Comment
    /// </summary>
    [EnumMember(Value = "FBCOMMENT")]
    FacebookComment = 10,

    /// <summary>
    /// IG Comment
    /// </summary>
    [EnumMember(Value = "IGCOMMENT")]
    InstagramComment = 11,
}
