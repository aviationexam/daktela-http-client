using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class NullableEnumsConverter<TTargetType, TEnum> : JsonConverter<TTargetType>
    where TEnum : struct, Enum
{
    private readonly JsonConverter<TEnum> _innerConverter;

    public override bool HandleNull => true;

    public NullableEnumsConverter(JsonSerializerOptions options)
    {
        Debug.Assert(typeof(TTargetType) == typeof(TEnum?), $"{typeof(TTargetType)} != {typeof(TEnum?)}");

        _innerConverter = (JsonConverter<TEnum>?) options.GetConverter(typeof(TEnum)) ?? new EnumsConverter<TEnum>(options);
    }

    public override TTargetType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(TTargetType), $"{typeToConvert} != {typeof(TTargetType)}");

        if (reader.TokenType == JsonTokenType.Null)
        {
            return default;
        }

        if (
            _innerConverter.Read(
                ref reader,
                Nullable.GetUnderlyingType(typeToConvert)!,
                options
            ) is TTargetType value
        )
        {
            return value;
        }

        return default;
    }

    public override void Write(Utf8JsonWriter writer, TTargetType value, JsonSerializerOptions options)
    {
        if (value is TEnum nullableValue)
        {
            _innerConverter.Write(writer, nullableValue, options);
        }
        else
        {
            writer.WriteStringValue((string?) null);
        }
    }
}
