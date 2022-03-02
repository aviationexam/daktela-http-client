using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public enum ESortDirection
{
    [EnumMember(Value = "desc")]
    Desc,

    [EnumMember(Value = "asc")]
    Asc,
}
