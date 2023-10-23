using Aviationexam.GeneratedJsonConverters.Attributes;
using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Requests;

[EnumJsonConverter]
public enum ESortDirection
{
    [EnumMember(Value = "desc")]
    Desc,

    [EnumMember(Value = "asc")]
    Asc,
}
