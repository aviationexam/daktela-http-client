using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces.Queries;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
public class RequestsBuilderTests
{
    [Fact]
    public void CreatePagedRequest()
    {
        var request = RequestBuilder.CreatePagedQuery(new Paging(0, 20));

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is IFilteringQuery);
        Assert.False(request is ISortableQuery);
    }

    [Fact]
    public void CreateSortedRequest()
    {
        var request = RequestBuilder.CreateSortableQuery(new List<Sorting>());

        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
        Assert.False(request is IPagedQuery);
    }

    [Fact]
    public void CreateFilteringQuery()
    {
        var request = RequestBuilder.CreateFilteringQuery(new Filter());

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IPagedQuery);
    }

    [Fact]
    public void CreatePagedRequest_Sorted()
    {
        var request = RequestBuilder.CreatePagedQuery(new Paging(0, 20))
            .WithSortable(new List<Sorting>());

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreatePagedRequest_Sorted_Filtered()
    {
        var request = RequestBuilder.CreatePagedQuery(new Paging(0, 20))
            .WithSortable(new List<Sorting>())
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreatePagedRequest_Filtered()
    {
        var request = RequestBuilder.CreatePagedQuery(new Paging(0, 20))
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.False(request is ISortableQuery);
    }

    [Fact]
    public void CreatePagedRequest_Filtered_Sorted()
    {
        var request = RequestBuilder.CreatePagedQuery(new Paging(0, 20))
            .WithFilter(new Filter())
            .WithSortable(new List<Sorting>());

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateSortedRequest_Filtered()
    {
        var sortableQuery = RequestBuilder.CreateSortableQuery(new List<Sorting>())
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<ISortableQuery>(sortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(sortableQuery);
        Assert.False(sortableQuery is IPagedQuery);
    }

    [Fact]
    public void CreateSortedRequest_Filtered_Paged()
    {
        var sortableQuery = RequestBuilder.CreateSortableQuery(new List<Sorting>())
            .WithFilter(new Filter())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<ISortableQuery>(sortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(sortableQuery);
        Assert.IsAssignableFrom<IPagedQuery>(sortableQuery);
    }

    [Fact]
    public void CreateSortedRequest_Paged()
    {
        var sortableQuery = RequestBuilder.CreateSortableQuery(new List<Sorting>())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<ISortableQuery>(sortableQuery);
        Assert.IsAssignableFrom<IPagedQuery>(sortableQuery);
        Assert.False(sortableQuery is IFilteringQuery);
    }

    [Fact]
    public void CreateSortedRequest_Paged_Filtered()
    {
        var sortableQuery = RequestBuilder.CreateSortableQuery(new List<Sorting>())
            .WithPaging(new Paging(0, 20))
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<ISortableQuery>(sortableQuery);
        Assert.IsAssignableFrom<IPagedQuery>(sortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(sortableQuery);
    }

    [Fact]
    public void CreateFilteringQuery_Paged()
    {
        var request = RequestBuilder.CreateFilteringQuery(new Filter())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
    }

    [Fact]
    public void CreateFilteringQuery_Paged_Sorted()
    {
        var request = RequestBuilder.CreateFilteringQuery(new Filter())
            .WithPaging(new Paging(0, 20))
            .WithSortable(new List<Sorting>());

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
    }

    [Fact]
    public void CreateFilteringQuery_Sorted()
    {
        var request = RequestBuilder.CreateFilteringQuery(new Filter())
            .WithSortable(new List<Sorting>());

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IPagedQuery);
    }

    [Fact]
    public void CreateFilteringQuery_Sorted_Paged()
    {
        var request = RequestBuilder.CreateFilteringQuery(new Filter())
            .WithSortable(new List<Sorting>())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
    }
}
