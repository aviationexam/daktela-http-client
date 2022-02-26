using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public enum EQueueType
{
    /// <summary>
    /// Calls inbound
    /// </summary>
    [EnumMember(Value = "in")]
    Inbound,

    /// <summary>
    /// Web Click-To-Call
    /// </summary>
    [EnumMember(Value = "ctc")]
    ClickToCall,

    /// <summary>
    /// Calls outbound
    /// </summary>
    [EnumMember(Value = "out")]
    Outbound,

    /// <summary>
    /// Preview (manual) campaign
    /// </summary>
    [EnumMember(Value = "outbounder")]
    OutBounder,

    /// <summary>
    /// Progressive campaign
    /// </summary>
    [EnumMember(Value = "progressive")]
    Progressive,

    /// <summary>
    /// Predictive campaign (dialer)
    /// </summary>
    [EnumMember(Value = "dialer")]
    Dialer,

    /// <summary>
    /// Robocaller
    /// </summary>
    [EnumMember(Value = "interviewer")]
    Robocaller,

    /// <summary>
    /// Email
    /// </summary>
    [EnumMember(Value = "email")]
    Email,

    /// <summary>
    /// SMS
    /// </summary>
    [EnumMember(Value = "sms")]
    Sms,

    /// <summary>
    /// Web chat
    /// </summary>
    [EnumMember(Value = "chat")]
    WebChat,

    /// <summary>
    /// Facebook Messenger
    /// </summary>
    [EnumMember(Value = "fbm")]
    FacebookMessenger,

    /// <summary>
    /// WhatsApp
    /// </summary>
    [EnumMember(Value = "wap")]
    WhatsApp,

    /// <summary>
    /// Viber
    /// </summary>
    [EnumMember(Value = "vbr")]
    Viber,

    /// <summary>
    /// Custom
    /// </summary>
    [EnumMember(Value = "custom")]
    Custom,
}
