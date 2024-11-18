using Daktela.HttpClient.Implementations.JsonConverters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.JsonConverters;

public class TimeSpanConverterTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public TimeSpanConverterTests()
    {
        _jsonSerializerOptions = new JsonSerializerOptions();
    }

    [Fact]
    public async Task DeserializeEmptyWorks()
    {
        AddTimeSpanConverter();

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync("{}");
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions);

        Assert.NotNull(parsedObject);
        Assert.Equal(default, parsedObject.TimeSpan);
        Assert.Null(parsedObject.NullableTimeSpan);
    }

    [Fact]
    public async Task SerializeEmptyWorks()
    {
        AddTimeSpanConverter();

        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract(), _jsonSerializerOptions);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync();

        Assert.NotNull(jsonContract);
        Assert.Equal(@"{""delay"":0,""nullable-delay"":null}", jsonContract);
    }

    [Theory]
    [ClassData(typeof(TimeSpans))]
    public async Task DeserializeContractWorks(int input, TimeSpan timeSpan)
    {
        AddTimeSpanConverter();

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync($@"{{""delay"":{input},""nullable-delay"":{input}}}");
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions);

        Assert.NotNull(parsedObject);
        Assert.Equal(timeSpan, parsedObject.TimeSpan);
        Assert.Equal(timeSpan, parsedObject.NullableTimeSpan);
    }

    [Theory]
    [ClassData(typeof(TimeSpans))]
    public async Task SerializeContractWorks(int input, TimeSpan timeSpan)
    {
        AddTimeSpanConverter();

        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract
        {
            TimeSpan = timeSpan,
            NullableTimeSpan = timeSpan,
        }, _jsonSerializerOptions);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync();

        Assert.NotNull(jsonContract);
        Assert.Equal($@"{{""delay"":{input},""nullable-delay"":{input}}}", jsonContract);
    }

    private void AddTimeSpanConverter()
    {
        _jsonSerializerOptions.Converters.Add(new TimeSpanConverter());
    }

    private class TimeSpans : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [0, TimeSpan.Zero];
            yield return [90, TimeSpan.FromSeconds(90)];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private class Contract
    {
        [JsonPropertyName("delay")]
        public TimeSpan TimeSpan { get; set; }

        [JsonPropertyName("nullable-delay")]
        public TimeSpan? NullableTimeSpan { get; set; }
    }
}
