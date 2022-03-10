using Daktela.HttpClient.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonConverters;

public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    private readonly DaktelaOptions _daktelaOptions;

    public DateTimeOffsetConverter(IOptions<DaktelaOptions> daktelaOptions)
    {
        _daktelaOptions = daktelaOptions.Value;
    }

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(DateTimeOffset));

        if (reader.ValueSpan.Length < 19)
        {
            throw new FormatException();
        }

        var span = new byte[19].AsSpan();
        reader.ValueSpan[5..7].CopyTo(span[..2]);
        span[2] = (byte) '/';
        reader.ValueSpan[8..10].CopyTo(span[3..5]);
        span[5] = (byte) '/';
        reader.ValueSpan[0..4].CopyTo(span[6..10]);
        reader.ValueSpan[10..19].CopyTo(span[10..]);

        if (Utf8Parser.TryParse(span, out DateTimeOffset value, out _, 'G'))
        {
            return new DateTimeOffset(value.DateTime, _daktelaOptions.DateTimeOffset!.Value);
        }

        throw new FormatException();
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        // The "G" format without offset will always be 19 bytes.
        var utf8Date = new byte[19].AsSpan();

        var result = Utf8Formatter.TryFormat(value, utf8Date, out _, new StandardFormat('G'));
        Debug.Assert(result);

        var span = new byte[19].AsSpan();
        utf8Date[6..10].CopyTo(span[0..4]);
        span[4] = (byte) '-';
        utf8Date[..2].CopyTo(span[5..7]);
        span[7] = (byte) '-';
        utf8Date[3..5].CopyTo(span[8..10]);

        utf8Date[10..19].CopyTo(span[10..]);

        writer.WriteStringValue(span);
    }
}
