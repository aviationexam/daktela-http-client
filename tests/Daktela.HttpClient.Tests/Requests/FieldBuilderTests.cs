using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations;
using System.Linq;
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
}
