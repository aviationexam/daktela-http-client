using Daktela.HttpClient.Api.CustomFields;
using Daktela.HttpClient.Implementations.JsonConverters;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.JsonConverters;

public class CustomFieldsConverterTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CustomFieldsConverterTests()
    {
        _jsonSerializerOptions = new JsonSerializerOptions();
        _jsonSerializerOptions.Converters.Add(new CustomFieldsConverter());
    }

    [Fact]
    public async Task DeserializeEmptyWorks()
    {
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync("{}");
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions);

        Assert.NotNull(parsedObject);
        Assert.Null(parsedObject.CustomFields);
        Assert.Null(parsedObject.NullableCustomFields);
    }

    [Fact]
    public async Task DeserializeEmptyAsArrayWorks()
    {
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(@"{""custom-fields"":[],""nullable-custom-fields"":[]}");
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions);

        Assert.NotNull(parsedObject);
        Assert.Empty(Assert.IsType<CustomFields>(parsedObject.CustomFields));
        Assert.Empty(Assert.IsType<CustomFields>(parsedObject.NullableCustomFields));
    }

    [Fact]
    public async Task SerializeEmptyWorks()
    {
        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract(), _jsonSerializerOptions);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync();

        Assert.NotNull(jsonContract);
        Assert.Equal(@"{""custom-fields"":null,""nullable-custom-fields"":null}", jsonContract);
    }

    [Fact]
    public async Task DeserializeContractWorks()
    {
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(@"{""custom-fields"":{""A"":[""b""]},""nullable-custom-fields"":{""C"":[""d""]}}");
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions);

        Assert.NotNull(parsedObject);
        var customFields = Assert.IsType<CustomFields>(parsedObject.CustomFields);
        var nullableCustomFields = Assert.IsType<CustomFields>(parsedObject.NullableCustomFields);
        Assert.Equal("b", Assert.Single(Assert.Single(customFields, x => x.Key == "A").Value));
        Assert.Equal("d", Assert.Single(Assert.Single(nullableCustomFields, x => x.Key == "C").Value));
    }

    [Fact]
    public async Task SerializeContractWorks()
    {
        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract
        {
            CustomFields = new CustomFields { ["A"] = ["b"] },
            NullableCustomFields = new CustomFields { ["C"] = ["d"] },
        }, _jsonSerializerOptions);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync();

        Assert.NotNull(jsonContract);
        Assert.Equal(@"{""custom-fields"":{""A"":[""b""]},""nullable-custom-fields"":{""C"":[""d""]}}", jsonContract);
    }

    private class Contract
    {
        [JsonPropertyName("custom-fields")]
        public ICustomFields CustomFields { get; set; } = null!;

        [JsonPropertyName("nullable-custom-fields")]
        public ICustomFields? NullableCustomFields { get; set; }
    }
}
