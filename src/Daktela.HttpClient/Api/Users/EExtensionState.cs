using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Users;

[EnumJsonConverter]
public enum EExtensionState : byte
{
    /// <summary>
    /// Online
    /// </summary>
    [EnumMember(Value = "online")]
    Online,

    /// <summary>
    /// Offline
    /// </summary>
    [EnumMember(Value = "offline")]
    Offline,

    /// <summary>
    /// Busy
    /// </summary>
    [EnumMember(Value = "busy")]
    Busy,
}
