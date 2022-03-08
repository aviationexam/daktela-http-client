using Daktela.HttpClient.Implementations;
using System.Text.Json.Serialization;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class PathBuilderTests
{
    [Fact]
    public void BuildSimpleWorks()
    {
        Assert.Equal("name", PathBuilder<BaseWithAttribute>.Build(x => x.Name));
    }

    [Fact]
    public void BuildExtendedWorks()
    {
        Assert.Equal("name2", PathBuilder<MiddleExtensionWithAttribute>.Build(x => x.Name));
        Assert.Equal("name", PathBuilder<MiddleExtensionWithSameAttribute>.Build(x => x.Name));
    }

    [Fact]
    public void BuildExtendedSameWorks()
    {
        Assert.Equal("Name", PathBuilder<ExtensionWithSameAttribute>.Build(x => x.Name));
    }

    [Fact]
    public void BuildExtendedNotSameWorks()
    {
        Assert.Equal("Name", PathBuilder<ExtensionWithAttribute>.Build(x => x.Name));
    }

    [Fact]
    public void BuildComplexWorks()
    {
        Assert.Equal("inner.name", PathBuilder<ComplexType>.Build(x => x.InnerType.Name));
    }

    private class BaseWithAttribute
    {
        [JsonPropertyName("name")]
        public string Name { get; } = null!;
    }

    private class MiddleExtensionWithAttribute : BaseWithAttribute
    {
        [JsonPropertyName("name2")]
        public new string Name { get; } = null!;
    }

    private class MiddleExtensionWithSameAttribute : BaseWithAttribute
    {
        [JsonPropertyName("name")]
        public new string Name { get; } = null!;
    }

    private class ExtensionWithAttribute : MiddleExtensionWithAttribute
    {
        public new string Name { get; } = null!;
    }

    private class ExtensionWithSameAttribute : MiddleExtensionWithSameAttribute
    {
        public new string Name { get; } = null!;
    }

    private class ComplexType
    {
        [JsonPropertyName("inner")]
        public BaseWithAttribute InnerType { get; } = null!;
    }
}
