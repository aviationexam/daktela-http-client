using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

[EnumJsonConverter]
public enum EAction : byte
{
    /// <summary>
    /// No action
    /// </summary>
    [EnumMember(Value = "")]
    NoAction = 0,

    /// <summary>
    /// Waiting
    /// </summary>
    [EnumMember(Value = "WAIT")]
    Wait = 1,

    /// <summary>
    /// Open
    /// </summary>
    [EnumMember(Value = "OPEN")]
    Open = 2,

    /// <summary>
    /// Postpone
    /// </summary>
    [EnumMember(Value = "POSTPONE")]
    Postpone = 3,

    /// <summary>
    /// Closed
    /// </summary>
    [EnumMember(Value = "CLOSE")]
    Close = 4,
}
