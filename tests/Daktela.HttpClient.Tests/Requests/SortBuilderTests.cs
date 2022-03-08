using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class SortBuilderTests
{
    [Theory]
    [InlineData(ESortDirection.Asc)]
    [InlineData(ESortDirection.Desc)]
    public void SimpleSortWithWorks(ESortDirection sortDirection)
    {
        var sorting = SortBuilder<Contact>.With(x => x.Name, sortDirection);

        var sortingImpl = Assert.IsType<Sorting>(sorting);
        Assert.Equal("name", sortingImpl.Field);
        Assert.Equal(sortDirection, sortingImpl.Dir);
    }

    [Fact]
    public void SimpleSortAscWithWorks()
    {
        var sorting = SortBuilder<Contact>.Ascending(x => x.Name);

        var sortingImpl = Assert.IsType<Sorting>(sorting);
        Assert.Equal("name", sortingImpl.Field);
        Assert.Equal(ESortDirection.Asc, sortingImpl.Dir);
    }

    [Fact]
    public void SimpleSortDescWithWorks()
    {
        var sorting = SortBuilder<Contact>.Descending(x => x.Name);

        var sortingImpl = Assert.IsType<Sorting>(sorting);
        Assert.Equal("name", sortingImpl.Field);
        Assert.Equal(ESortDirection.Desc, sortingImpl.Dir);
    }

    [Theory]
    [InlineData(ESortDirection.Asc)]
    [InlineData(ESortDirection.Desc)]
    public void ComplexSortWithWorks(ESortDirection sortDirection)
    {
        var sorting = SortBuilder<Contact>.With(x => x.User!.Name, sortDirection);

        var sortingImpl = Assert.IsType<Sorting>(sorting);
        Assert.Equal("user.name", sortingImpl.Field);
        Assert.Equal(sortDirection, sortingImpl.Dir);
    }
}
