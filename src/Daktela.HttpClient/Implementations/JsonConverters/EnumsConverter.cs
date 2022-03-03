using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class EnumsConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private readonly IDictionary<string, TEnum> _mapping;
    private readonly IDictionary<TEnum, string> _reverseMapping;
    private readonly Type _enumType;

    public EnumsConverter(
        [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = $"The parameter required in the {nameof(EnumsConverterFactory)}")]
        JsonSerializerOptions options
    )
    {
        _enumType = typeof(TEnum);

        _mapping = Enum.GetValues<TEnum>().ToDictionary(
            x => _enumType.GetMember(x.ToString())
                .First()
                .GetCustomAttributes<EnumMemberAttribute>(true)
                .Select(ema => ema.Value)
                .FirstOrDefault() ?? throw new ArgumentException(
                $"The {_enumType.Name}.{x} does not define the {typeof(EnumMemberAttribute)} attribute with {nameof(EnumMemberAttribute.Value)} field"
            ),
            x => x
        );

        _reverseMapping = _mapping.ToDictionary(x => x.Value, x => x.Key);
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == _enumType, $"{typeToConvert} != {_enumType}");

        var value = reader.GetString();

        if (value != null && _mapping.ContainsKey(value))
        {
            return _mapping[value];
        }

        throw new FormatException($"Unable to deserialize {value} into the {typeof(TEnum).Name}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (_reverseMapping.ContainsKey(value))
        {
            writer.WriteStringValue(_reverseMapping[value]);

            return;
        }

        throw new FormatException($"Unable to serialize {typeof(TEnum).Name}.{value}");
    }
}
