using Daktela.HttpClient.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations.JsonParsers;

public class DateTimeOffsetParser : JsonConverter<DateTimeOffset>
{
    private readonly DaktelaOptions _daktelaOptions;

    public DateTimeOffsetParser(IOptions<DaktelaOptions> daktelaOptions)
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
            return new DateTimeOffset(value.DateTime, _daktelaOptions.DateTimeOffset);
        }

        throw new FormatException();
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        // The "G" format without offset will always be 19 bytes.
        var utf8Date = new byte[19].AsSpan();

        var result = Utf8Formatter.TryFormat(value, utf8Date, out _, new StandardFormat('R'));
        Debug.Assert(result);

        for (var i = 0; i < utf8Date.Length; i++)
        {
            if (utf8Date[i] == '/')
            {
                utf8Date[i] = (byte) '-';
            }
        }

        writer.WriteStringValue(utf8Date);
    }
}
