using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Integrations;

[EnumJsonConverter]
public enum EType : byte
{
    /// <summary>
    /// Authentication
    /// </summary>
    [EnumMember(Value = "AUTH")]
    Auth,

    /// <summary>
    /// CRM synchronization
    /// </summary>
    [EnumMember(Value = "SYNC")]
    Synchronization,

    /// <summary>
    /// External libraries
    /// </summary>
    [EnumMember(Value = "EXTLIB")]
    ExternalLibrary,
}
