using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Users;

[EnumJsonConverter]
public enum ECanTransferCall : byte
{
    /// <summary>
    /// None
    /// </summary>
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Only assisted transfer
    /// </summary>
    [EnumMember(Value = "only assisted transfer")]
    OnlyAssistedTransfer,

    /// <summary>
    /// Only blind transfer
    /// </summary>
    [EnumMember(Value = "only blind transfer")]
    OnlyBlindTransfer,

    /// <summary>
    /// Blind and assisted transfer
    /// </summary>
    [EnumMember(Value = "blind and assisted transfer")]
    BlindAndAssistedTransfer,
}
