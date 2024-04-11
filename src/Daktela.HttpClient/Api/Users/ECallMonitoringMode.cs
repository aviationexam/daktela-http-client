using Aviationexam.GeneratedJsonConverters;
using Aviationexam.GeneratedJsonConverters.Attributes;

namespace Daktela.HttpClient.Api.Users;

[EnumJsonConverter(SerializationStrategy = EnumSerializationStrategy.BackingType, DeserializationStrategy = EnumDeserializationStrategy.UseBackingType)]
public enum ECallMonitoringMode : byte
{
    /// <summary>
    /// None
    /// </summary>
    None,

    /// <summary>
    /// Monitor only
    /// </summary>
    MonitorOnly = 1,

    /// <summary>
    /// Whisper
    /// </summary>
    Whisper = 3,

    /// <summary>
    /// Whisper and barge
    /// </summary>
    WhisperAndBarge = 7,

    /// <summary>
    /// Whisper and barge
    /// </summary>
    WhisperBargeAndTakeOver = 15,
}
