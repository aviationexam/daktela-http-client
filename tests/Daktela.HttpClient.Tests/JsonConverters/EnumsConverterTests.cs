using Daktela.HttpClient.Api.Users;
using Daktela.HttpClient.Implementations.JsonConverters;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.JsonConverters;

public class EnumsConverterTests
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public EnumsConverterTests()
    {
        _jsonSerializerOptions = new JsonSerializerOptions();
        _jsonSerializerOptions.Converters.Add(new EnumsConverterFactory());
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
        Assert.Equal(default, parsedObject!.ExtensionState);
        Assert.Null(parsedObject.NullableExtensionState);
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
        Assert.Equal(@"{""extension-state"":""online"",""nullable-extension-state"":null}", jsonContract);
    }

    [Fact]
    public async Task DeserializeContractWorks()
    {
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(@"{""extension-state"":""offline"",""nullable-extension-state"":""busy""}");
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var parsedObject = await JsonSerializer.DeserializeAsync<Contract>(memoryStream, _jsonSerializerOptions);

        Assert.NotNull(parsedObject);
        Assert.Equal(EExtensionState.Offline, parsedObject!.ExtensionState);
        Assert.Equal(EExtensionState.Busy, parsedObject.NullableExtensionState);
    }

    [Fact]
    public async Task SerializeContractWorks()
    {
        await using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, new Contract
        {
            ExtensionState = EExtensionState.Offline,
            NullableExtensionState = EExtensionState.Busy,
        }, _jsonSerializerOptions);

        memoryStream.Seek(0, SeekOrigin.Begin);

        using var streamReader = new StreamReader(memoryStream);
        var jsonContract = await streamReader.ReadToEndAsync();

        Assert.NotNull(jsonContract);
        Assert.Equal(@"{""extension-state"":""offline"",""nullable-extension-state"":""busy""}", jsonContract);
    }

    private class Contract
    {
        [JsonPropertyName("extension-state")]
        public EExtensionState ExtensionState { get; set; }

        [JsonPropertyName("nullable-extension-state")]
        public EExtensionState? NullableExtensionState { get; set; }
    }
}
