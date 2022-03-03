using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public enum EFilterLogic
{
    [EnumMember(Value = "and")]
    And,

    [EnumMember(Value = "or")]
    Or,
}
