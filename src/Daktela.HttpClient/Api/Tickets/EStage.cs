using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public enum EStage : byte
{
    /// <summary>
    /// Open
    /// </summary>
    [EnumMember(Value = "OPEN")]
    Open = 0,

    /// <summary>
    /// Waiting
    /// </summary>
    [EnumMember(Value = "WAIT")]
    Wait = 1,

    /// <summary>
    /// Closed
    /// </summary>
    [EnumMember(Value = "CLOSE")]
    Close = 2,

    /// <summary>
    /// Archived
    /// </summary>
    [EnumMember(Value = "ARCHIVE")]
    Archive = 3,
}

public enum EPriority : byte
{
    /// <summary>
    /// Low
    /// </summary>
    [EnumMember(Value = "LOW")]
    Low = 0,

    /// <summary>
    /// Medium
    /// </summary>
    [EnumMember(Value = "MEDIUM")]
    Medium = 1,

    /// <summary>
    /// High
    /// </summary>
    [EnumMember(Value = "HIGH")]
    High = 2,
}
