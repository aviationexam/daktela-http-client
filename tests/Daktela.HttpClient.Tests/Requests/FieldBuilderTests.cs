using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces.Requests;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class FieldBuilderTests
{
    [Fact]
    public void FieldCreateWorks()
    {
        var fields = FieldBuilder<ReadContact>.Create(
            x => x.Name,
            x => x.Title,
            x => x.Account!.Name
        );

        var fieldsImpl = Assert.IsType<Fields>(fields);
        var items = fieldsImpl.Items.ToArray();
        Assert.Equal(3, items.Length);
        Assert.Equal("name", items[0]);
        Assert.Equal("title", items[1]);
        Assert.Equal("account.name", items[2]);
    }

    [Fact]
    public void FieldCreateWithConvertWorks()
    {
        var fields = FieldBuilder<ReadContact>.Create<dynamic>(
            x => x.Name,
            x => x.Edited
        );

        var fieldsImpl = Assert.IsType<Fields>(fields);
        var items = fieldsImpl.Items.ToArray();
        Assert.Equal(2, items.Length);
        Assert.Equal("name", items[0]);
        Assert.Equal("edited", items[1]);
    }

    [Fact]
    public void FieldCreateForWorks()
    {
        var fields = FieldBuilder<ReadContact>.CreateFor<ActivityField>();

        var fieldsImpl = Assert.IsType<Fields>(fields);
        var items = fieldsImpl.Items.ToArray();
        Assert.Equal(2, items.Length);
        Assert.Equal("name", items[0]);
        Assert.Equal("edited", items[1]);
    }

    [Fact]
    public void FieldCreateForFailed_BadName()
    {
        var exception = Assert.Throws<DaktelaFieldsException<ReadContact, ActivityFieldBadName>>(FieldBuilder<ReadContact>.CreateFor<ActivityFieldBadName>);

        Assert.Equal("The property Name2 from the ActivityFieldBadName does not exist in the ReadContact", exception.Message);
        Assert.Null(exception.SourceJsonName);
        Assert.Null(exception.TargetJsonName);
    }

    [Fact]
    public void FieldCreateForFailed_BadJsonName()
    {
        var exception = Assert.Throws<DaktelaFieldsException<ReadContact, ActivityFieldBadJsonName>>(FieldBuilder<ReadContact>.CreateFor<ActivityFieldBadJsonName>);

        Assert.Equal("The property Name from the ActivityFieldBadJsonName has different JSON name in the ReadContact. Source:name <> Target:name2", exception.Message);
        Assert.NotNull(exception.SourceJsonName);
        Assert.NotNull(exception.TargetJsonName);
        Assert.Equal("name", exception.SourceJsonName);
        Assert.Equal("name2", exception.TargetJsonName);
    }

    private class ActivityField : IFieldResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("edited")]
        public DateTimeOffset? Edited
        {
            get;
            [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
            set;
        }
    }

    private class ActivityFieldBadName : IFieldResult
    {
        [JsonPropertyName("name")]
        public string Name2 { get; set; } = null!;
    }

    private class ActivityFieldBadJsonName : IFieldResult
    {
        [JsonPropertyName("name2")]
        public string Name { get; set; } = null!;
    }
}
