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
    private readonly Type _enumType;

    public IDictionary<string, TEnum> Mapping { get; }
    public IDictionary<TEnum, string> ReverseMapping { get; }

    public EnumsConverter()
    {
        _enumType = typeof(TEnum);

        Mapping = Enum.GetValues<TEnum>().ToDictionary(
            x => _enumType.GetMember(x.ToString())
                .First()
                .GetCustomAttributes<EnumMemberAttribute>(true)
                .Select(ema => ema.Value)
                .FirstOrDefault() ?? throw new ArgumentException(
                $"The {_enumType.Name}.{x} does not define the {typeof(EnumMemberAttribute)} attribute with {nameof(EnumMemberAttribute.Value)} field"
            ),
            x => x
        );

        ReverseMapping = Mapping.ToDictionary(x => x.Value, x => x.Key);
    }

    public EnumsConverter(
        [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification = $"The parameter required in the {nameof(EnumsConverterFactory)}")]
        JsonSerializerOptions options
    ) : this()
    {
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == _enumType, $"{typeToConvert} != {_enumType}");

        var value = reader.GetString();

        if (value != null && Mapping.ContainsKey(value))
        {
            return Mapping[value];
        }

        throw new FormatException($"Unable to deserialize {value} into the {typeof(TEnum).Name}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (ReverseMapping.ContainsKey(value))
        {
            writer.WriteStringValue(ReverseMapping[value]);

            return;
        }

        throw new FormatException($"Unable to serialize {typeof(TEnum).Name}.{value}");
    }
}
