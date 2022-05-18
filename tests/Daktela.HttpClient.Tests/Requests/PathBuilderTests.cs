using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Implementations;
using System.Text.Json.Serialization;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class PathBuilderTests
{
    private const string CustomFieldKey = "abcd";

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

    [Fact]
    public void BuildNestedCustomFieldsWorks()
    {
        Assert.Equal("ticket.customFields.abc", PathBuilder<ReadActivity>.Build(x => x.Ticket.CustomFields!["abc"]));
    }

    [Fact]
    public void BuildNestedCustomFieldsConstWorks()
    {
        Assert.Equal("ticket.customFields.abcd", PathBuilder<ReadActivity>.Build(x => x.Ticket.CustomFields![CustomFieldKey]));

        Assert.Equal("ticket.customFields.abcd", PathBuilder<ReadActivity>.Build(x => x.Ticket.CustomFields![
            // ReSharper disable once ArrangeStaticMemberQualifier
            PathBuilderTests.CustomFieldKey
        ]));
    }

    [Fact]
    public void BuildNestedCustomFieldsVariableWorks()
    {
        // ReSharper disable once HeapView.ClosureAllocation
        // ReSharper disable once ConvertToConstant.Local
        var variable = "abce";

        Assert.Equal("ticket.customFields.abce", PathBuilder<ReadActivity>.Build(x => x.Ticket.CustomFields![variable]));
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
