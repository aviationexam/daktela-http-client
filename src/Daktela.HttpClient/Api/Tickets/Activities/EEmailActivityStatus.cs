using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

public enum EEmailActivityStatus : byte
{
    /// <summary>
    /// Sending failed
    /// </summary>
    [EnumMember(Value = "FAILED")]
    SendingFailed = 0,

    /// <summary>
    /// Sent
    /// </summary>
    [EnumMember(Value = "SENT")]
    Sent = 1,

    /// <summary>
    /// Waiting to be sent
    /// </summary>
    [EnumMember(Value = "WAIT")]
    WaitingToBeSent = 2,

    /// <summary>
    /// Draft
    /// </summary>
    [EnumMember(Value = "DRAFT")]
    Draft = 3,
}
