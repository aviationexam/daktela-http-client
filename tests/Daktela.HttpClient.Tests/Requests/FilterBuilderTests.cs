using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class FilterBuilderTests
{
    [Theory]
    [InlineData(EFilterOperator.Equal)]
    [InlineData(EFilterOperator.EndsWith)]
    public void SimpleFilterWithWorks(EFilterOperator filterOperator)
    {
        var filter = FilterBuilder<ReadContact>.WithValue(x => x.Name, filterOperator, "a value");

        var filterImpl = Assert.IsType<Filter>(filter);
        Assert.Equal("name", filterImpl.Field);
        Assert.Equal(filterOperator, filterImpl.Operator);
        Assert.Equal("a value", filterImpl.Value);
        Assert.Null(filterImpl.Type);
    }

    [Theory]
    [InlineData(EFilterLogic.And)]
    [InlineData(EFilterLogic.Or)]
    public void FilterGroupWorks(EFilterLogic filterLogic)
    {
        var filter = FilterBuilder<ReadContact>.WithGroupOfValue(filterLogic, new[] { FilterBuilder<ReadContact>.WithValue(x => x.Name, EFilterOperator.Equal, "a value") });

        var filterGroup = Assert.IsType<FilterGroup>(filter);
        Assert.Equal(filterLogic, filterGroup.Logic);
        var valueFilter = Assert.IsType<Filter>(Assert.Single(filterGroup.Filters));

        Assert.Equal("name", valueFilter.Field);
        Assert.Equal(EFilterOperator.Equal, valueFilter.Operator);
        Assert.Equal("a value", valueFilter.Value);
        Assert.Null(valueFilter.Type);
    }
}
