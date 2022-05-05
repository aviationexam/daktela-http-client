using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(TimeSpan));

        var value = reader.GetInt64();

        return TimeSpan.FromSeconds(value);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.TotalSeconds);
    }
}
