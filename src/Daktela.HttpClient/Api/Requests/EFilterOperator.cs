using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public enum EFilterOperator
{
    /// <summary>
    /// equal to
    /// </summary>
    [EnumMember(Value = "eq")]
    Equal,

    /// <summary>
    /// not equal to
    /// </summary>
    [EnumMember(Value = "neq")]
    NotEqual,

    /// <summary>
    /// number less than
    /// </summary>
    [EnumMember(Value = "lt")]
    Less,

    /// <summary>
    /// number less than or equal to
    /// </summary>
    [EnumMember(Value = "lte")]
    LessOrEqual,

    /// <summary>
    /// number greater than
    /// </summary>
    [EnumMember(Value = "gt")]
    Greater,

    /// <summary>
    /// number greater than or equal to
    /// </summary>
    [EnumMember(Value = "gte")]
    GreaterOrEqual,

    /// <summary>
    /// string contains part of string
    /// </summary>
    [EnumMember(Value = "like")]
    Like,

    /// <summary>
    /// string contains part of string
    /// </summary>
    [EnumMember(Value = "contains")]
    Contains,

    /// <summary>
    /// string contains part of string
    /// </summary>
    [EnumMember(Value = "startswith")]
    StartsWith,

    /// <summary>
    /// string contains part of string
    /// </summary>
    [EnumMember(Value = "endswith")]
    EndsWith,

    /// <summary>
    /// string does not contain part of string
    /// </summary>
    [EnumMember(Value = "notlike")]
    NotLike,

    /// <summary>
    /// string does not contain part of string
    /// </summary>
    [EnumMember(Value = "doesnotcontain")]
    DoesNotContain,

    /// <summary>
    /// is exactly null
    /// </summary>
    [EnumMember(Value = "isnull")]
    IsNull,

    /// <summary>
    /// is not exactly null
    /// </summary>
    [EnumMember(Value = "isnotnull")]
    IsNotNull,

    /// <summary>
    /// from array
    /// </summary>
    [EnumMember(Value = "in")]
    In,

    /// <summary>
    ///  (not from array
    /// </summary>
    [EnumMember(Value = "notin")]
    NotIn,
}
