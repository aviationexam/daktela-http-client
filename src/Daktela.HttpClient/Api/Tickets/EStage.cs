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
