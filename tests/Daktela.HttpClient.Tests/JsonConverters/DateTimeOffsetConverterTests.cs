using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations.JsonConverters;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.JsonConverters;

public class DateTimeOffsetConverterTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public DateTimeOffsetConverterTests()
    {
        _jsonSerializerOptions = new JsonSerializerOptions();
    }

    [Fact]
    public async Task DeserializeEmptyWorks()
    {
        AddDateTimeOffsetConverter(TimeSpan.Zero);

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync("{}");
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions);

        Assert.NotNull(parsedObject);
        Assert.Equal(default, parsedObject!.DateTime);
        Assert.Null(parsedObject.NullableDateTime);
    }

    [Fact]
    public async Task SerializeEmptyWorks()
    {
        AddDateTimeOffsetConverter(TimeSpan.Zero);

        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract(), _jsonSerializerOptions);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync();

        Assert.NotNull(jsonContract);
        Assert.Equal(@"{""date-time"":""0001-01-01 00:00:00"",""nullable-date-time"":null}", jsonContract);
    }

    [Theory]
    [ClassData(typeof(TimeSpans))]
    public async Task DeserializeContractWorks(TimeSpan timeSpan)
    {
        AddDateTimeOffsetConverter(timeSpan);

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(@"{""date-time"":""2022-03-04 00:00:00"",""nullable-date-time"":""2022-03-04 00:00:00""}");
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions);

        Assert.NotNull(parsedObject);
        Assert.Equal(new DateTimeOffset(2022,3,4,0,0,0, timeSpan), parsedObject!.DateTime);
        Assert.Equal(new DateTimeOffset(2022,3,4,0,0,0, timeSpan), parsedObject.NullableDateTime);
    }

    [Theory]
    [ClassData(typeof(TimeSpans))]
    public async Task SerializeContractWorks(TimeSpan timeSpan)
    {
        AddDateTimeOffsetConverter(timeSpan);

        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract
        {
            DateTime = new DateTimeOffset(2022,3,4,0,0,0, timeSpan),
            NullableDateTime = new DateTimeOffset(2022,3,4,0,0,0, timeSpan),
        }, _jsonSerializerOptions);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync();

        Assert.NotNull(jsonContract);
        Assert.Equal(@"{""date-time"":""2022-03-04 00:00:00"",""nullable-date-time"":""2022-03-04 00:00:00""}", jsonContract);
    }

    private void AddDateTimeOffsetConverter(TimeSpan dateTimeOffset)
    {
        _jsonSerializerOptions.Converters.Add(new DateTimeOffsetConverter(new OptionsWrapper<DaktelaOptions>(new DaktelaOptions
        {
            DateTimeOffset = dateTimeOffset,
        })));
    }

    private class TimeSpans : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { TimeSpan.FromMinutes(-90) };
            yield return new object[] { TimeSpan.Zero };
            yield return new object[] { TimeSpan.FromMinutes(90) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private class Contract
    {
        [JsonPropertyName("date-time")]
        public DateTimeOffset DateTime { get; set; }

        [JsonPropertyName("nullable-date-time")]
        public DateTimeOffset? NullableDateTime { get; set; }
    }
}
