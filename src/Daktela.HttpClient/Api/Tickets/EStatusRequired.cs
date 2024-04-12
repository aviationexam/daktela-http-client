using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

[EnumJsonConverter]
public enum EStatusRequired : byte
{
    /// <summary>
    /// No action
    /// </summary>
    [EnumMember(Value = "")] NoAction = 0,

    /// <summary>
    /// Waiting
    /// </summary>
    [EnumMember(Value = "never")] Never = 1,

    /// <summary>
    /// When closing or archiving ticket
    /// </summary>
    [EnumMember(Value = "closing_ticket")] ClosingTicket = 2,

    /// <summary>
    /// On any user change
    /// </summary>
    [EnumMember(Value = "always")] Always = 3,
}
