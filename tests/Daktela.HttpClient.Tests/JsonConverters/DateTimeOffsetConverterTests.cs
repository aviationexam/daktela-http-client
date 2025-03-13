using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations.JsonConverters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.JsonConverters;

public class DateTimeOffsetConverterTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();

    [Fact]
    public async Task DeserializeEmptyWorks()
    {
        AddDateTimeOffsetConverter(TimeSpan.Zero);

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync("{}");
        await streamWriter.FlushAsync(TestContext.Current.CancellationToken);
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions, TestContext.Current.CancellationToken);

        Assert.NotNull(parsedObject);
        Assert.Equal(default, parsedObject.DateTime);
        Assert.Null(parsedObject.NullableDateTime);
    }

    [Fact]
    public async Task SerializeEmptyWorks()
    {
        AddDateTimeOffsetConverter(TimeSpan.Zero);

        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract(), _jsonSerializerOptions, TestContext.Current.CancellationToken);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync(TestContext.Current.CancellationToken);

        Assert.NotNull(jsonContract);
        Assert.Equal( /* lang=json */"""{"date-time":"0001-01-01 00:00:00","nullable-date-time":null}""", jsonContract);
    }

    [Theory]
    [ClassData(typeof(TimeSpans))]
    public async Task DeserializeContractTimeSpanWorks(TimeSpan timeSpan)
    {
        AddDateTimeOffsetConverter(timeSpan);

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(
            /* lang=json */"""{"date-time":"2022-03-04 00:00:00","nullable-date-time":"2022-03-04 00:00:00"}""");
        await streamWriter.FlushAsync(TestContext.Current.CancellationToken);
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions, TestContext.Current.CancellationToken);

        Assert.NotNull(parsedObject);
        Assert.Equal(new DateTimeOffset(2022, 3, 4, 0, 0, 0, timeSpan), parsedObject.DateTime);
        Assert.Equal(new DateTimeOffset(2022, 3, 4, 0, 0, 0, timeSpan), parsedObject.NullableDateTime);
    }

    [Theory]
    [ClassData(typeof(TimeZones))]
    public async Task DeserializeContractTimeZoneWorks(string timeZone)
    {
        AddDateTimeZoneConverter(timeZone);

        var timeZoneInstance = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(
            /* lang=json */"""{"date-time":"2022-03-04 00:00:00","nullable-date-time":"2022-03-04 00:00:00"}"""
        );
        await streamWriter.FlushAsync(TestContext.Current.CancellationToken);
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions, TestContext.Current.CancellationToken);

        Assert.NotNull(parsedObject);
        var dateTimeOffset = timeZoneInstance.GetUtcOffset(DateTimeOffset.Now);
        Assert.Equal(new DateTimeOffset(2022, 3, 4, 0, 0, 0, dateTimeOffset), parsedObject.DateTime);
        Assert.Equal(new DateTimeOffset(2022, 3, 4, 0, 0, 0, dateTimeOffset), parsedObject.NullableDateTime);
    }

    [Theory]
    [ClassData(typeof(TimeSpans))]
    public async Task SerializeContractTimeSpanWorks(TimeSpan timeSpan)
    {
        AddDateTimeOffsetConverter(timeSpan);

        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract
        {
            DateTime = new DateTimeOffset(2022, 3, 4, 0, 0, 0, timeSpan),
            NullableDateTime = new DateTimeOffset(2022, 3, 4, 0, 0, 0, timeSpan),
        }, _jsonSerializerOptions, TestContext.Current.CancellationToken);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync(TestContext.Current.CancellationToken);

        Assert.NotNull(jsonContract);
        Assert.Equal( /* lang=json */"""{"date-time":"2022-03-04 00:00:00","nullable-date-time":"2022-03-04 00:00:00"}""",
            jsonContract);
    }

    [Theory]
    [ClassData(typeof(TimeZones))]
    public async Task SerializeContractTimeZoneWorks(string timeZone)
    {
        AddDateTimeZoneConverter(timeZone);

        var timeZoneInstance = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

        await using var memoryStream = new MemoryStream();

        var dateTimeOffset = timeZoneInstance.GetUtcOffset(DateTimeOffset.Now);
        await JsonSerializer.SerializeAsync(memoryStream, new Contract
        {
            DateTime = new DateTimeOffset(2022, 3, 4, 0, 0, 0, dateTimeOffset),
            NullableDateTime = new DateTimeOffset(2022, 3, 4, 0, 0, 0, dateTimeOffset),
        }, _jsonSerializerOptions, TestContext.Current.CancellationToken);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync(TestContext.Current.CancellationToken);

        Assert.NotNull(jsonContract);
        Assert.Equal(
            /* lang=json */ """{"date-time":"2022-03-04 00:00:00","nullable-date-time":"2022-03-04 00:00:00"}""",
            jsonContract
        );
    }

    private void AddDateTimeOffsetConverter(TimeSpan dateTimeOffset)
    {
        _jsonSerializerOptions.Converters.Add(new DateTimeOffsetConverter(dateTimeOffset));
    }

    private void AddDateTimeZoneConverter(string dateTimeZone)
    {
        var options = new DaktelaOptions
        {
            DateTimeTimezone = dateTimeZone,
        };

        _jsonSerializerOptions.Converters.Add(new DateTimeOffsetConverter(options.DateTimeOffset!.Value));
    }

    private class TimeSpans : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [TimeSpan.FromMinutes(-90)];
            yield return [TimeSpan.Zero];
            yield return [TimeSpan.FromMinutes(90)];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private class TimeZones : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return ["UTC"];

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                yield return ["Central Europe Standard Time"];
                yield return ["Eastern Standard Time"];
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                yield return ["America/New_York"];
                yield return ["Europe/Prague"];
            }
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
