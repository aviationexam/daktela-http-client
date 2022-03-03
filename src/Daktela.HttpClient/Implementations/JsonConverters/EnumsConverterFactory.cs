using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class EnumsConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(
        Type typeToConvert
    )
    {
        if (IsNullable(typeToConvert))
        {
            typeToConvert = Nullable.GetUnderlyingType(typeToConvert)!;
        }

        return typeToConvert.IsEnum
               && typeToConvert.Namespace != null
               && typeToConvert.Namespace.StartsWith("Daktela.HttpClient.Api");
    }

    private bool IsNullable(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = IsNullable(typeToConvert)
            ? typeof(NullableEnumsConverter<,>).MakeGenericType(typeToConvert, Nullable.GetUnderlyingType(typeToConvert)!)
            : typeof(EnumsConverter<>).MakeGenericType(typeToConvert);

        return (JsonConverter?) Activator.CreateInstance(
            converterType,
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { options },
            culture: null
        ) ?? throw new NullReferenceException($"Unable to create {nameof(JsonConverter)}");
    }
}
