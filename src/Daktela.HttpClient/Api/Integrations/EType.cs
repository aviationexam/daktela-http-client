using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Integrations;

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
