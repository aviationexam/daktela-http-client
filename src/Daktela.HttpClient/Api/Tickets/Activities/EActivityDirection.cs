using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets.Activities;

[EnumJsonConverter]
public enum EActivityDirection : byte
{
    /// <summary>
    /// Incoming
    /// </summary>
    [EnumMember(Value = "in")]
    Incoming = 0,

    /// <summary>
    /// Outgoing
    /// </summary>
    [EnumMember(Value = "out")]
    Outgoing = 1,
}
