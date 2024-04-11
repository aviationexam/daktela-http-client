using Aviationexam.GeneratedJsonConverters;
using Aviationexam.GeneratedJsonConverters.Attributes;

namespace Daktela.HttpClient.Api.Users;

[EnumJsonConverter(SerializationStrategy = EnumSerializationStrategy.BackingType, DeserializationStrategy = EnumDeserializationStrategy.UseBackingType)]
public enum EGptFunctions : byte
{
    /// <summary>
    /// None
    /// </summary>
    None,

    /// <summary>
    /// Summary
    /// </summary>
    Summary = 1,

    /// <summary>
    /// Compose features
    /// </summary>
    ComposeFeatures = 2,

    /// <summary>
    /// Summary and compose features
    /// </summary>
    SummaryAndComposeFeatures = 3,
}
