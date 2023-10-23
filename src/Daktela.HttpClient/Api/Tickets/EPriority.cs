using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

[EnumJsonConverter]
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
