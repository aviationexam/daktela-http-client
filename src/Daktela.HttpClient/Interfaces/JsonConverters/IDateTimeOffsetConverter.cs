using System;
using System.Text.Json;

namespace Daktela.HttpClient.Interfaces.JsonConverters;

public interface IDateTimeOffsetConverter
{
    DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);

    void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options);
}
