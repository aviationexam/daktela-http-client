using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Users;

[EnumJsonConverter]
public enum EProfileType : byte
{
    /// <summary>
    /// None
    /// </summary>
    [EnumMember(Value = "none")] None,

    /// <summary>
    /// Summary
    /// </summary>
    [EnumMember(Value = "CC")] ContactCentre,

    /// <summary>
    /// Compose features
    /// </summary>
    [EnumMember(Value = "BO")] BackOffice,
}
