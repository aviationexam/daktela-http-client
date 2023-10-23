using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Users;

[EnumJsonConverter]
public enum ERecordAtCallStart : byte
{
    /// <summary>
    /// Do not record
    /// </summary>
    [EnumMember(Value = "disabled")]
    Disabled,

    /// <summary>
    /// Record from call start
    /// </summary>
    [EnumMember(Value = "immediately")]
    Immediately,

    /// <summary>
    /// Record after call is bridged
    /// </summary>
    [EnumMember(Value = "bridged")]
    Bridged,
}
