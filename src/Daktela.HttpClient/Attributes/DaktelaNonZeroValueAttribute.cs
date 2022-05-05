using System;

namespace Daktela.HttpClient.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class DaktelaNonZeroValueAttribute : DaktelaRequirementAttribute
{
    public double Epsilon { get; set; } = 0.000001;

    public bool AllowNull { get; set; } = false;

    public DaktelaNonZeroValueAttribute(EOperation applyOnOperation) : base(applyOnOperation)
    {
    }

    public bool IsValid(object? value)
    {
        if (value is null)
        {
            return AllowNull;
        }

        var type = value.GetType();

        if (type.IsPrimitive)
        {
            if (value is int intValue) return intValue != 0;
            if (value is uint uintValue) return uintValue != 0;
            if (value is float floatValue) return Math.Abs(floatValue) > Epsilon;
            if (value is double doubleValue) return Math.Abs(doubleValue) > Epsilon;
            if (value is short shortValue) return shortValue != 0;
            if (value is ushort ushortValue) return ushortValue != 0;
            if (value is long longValue) return longValue != 0;
            if (value is ulong ulongValue) return ulongValue != 0;
            if (value is sbyte sbyteValue) return sbyteValue != 0;
            if (value is byte byteValue) return byteValue != 0;
            if (value is char charValue) return charValue != 0;
            if (value is bool boolValue) return boolValue;
        }

        if (value is decimal decimalValue) return Math.Abs((double) decimalValue) > Epsilon;
        if (value is string stringValue) return !string.IsNullOrEmpty(stringValue);
        if (value is Guid guidValue) return guidValue != Guid.Empty;
        if (value is TimeSpan timeSpanValue) return timeSpanValue.Ticks > 0;

        return false;
    }
}
