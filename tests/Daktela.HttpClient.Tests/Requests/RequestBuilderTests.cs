using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces.Queries;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
public class RequestBuilderTests
{
    [Fact]
    public void CreateEmptyRequest()
    {
        var request = RequestBuilder.CreateEmpty();

        Assert.False(request is IPagedQuery);
        Assert.False(request is IFilteringQuery);
        Assert.False(request is ISortableQuery);
    }

    [Fact]
    public void CreateEmptyRequest_Paged()
    {
        var request = RequestBuilder.CreateEmpty()
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is IFilteringQuery);
        Assert.False(request is ISortableQuery);
    }

    [Fact]
    public void CreateEmptyRequest_Sorted()
    {
        var request = RequestBuilder.CreateEmpty()
            .WithSortable(Array.Empty<ISorting>());

        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
        Assert.False(request is IPagedQuery);
    }

    [Fact]
    public void CreateEmptyRequest_Filtering()
    {
        var request = RequestBuilder.CreateEmpty()
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IPagedQuery);
    }

    [Fact]
    public void CreatePagedRequest()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20));

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is IFilteringQuery);
        Assert.False(request is ISortableQuery);
    }

    [Fact]
    public void CreateSortedRequest()
    {
        var request = RequestBuilder.CreateSortable(Array.Empty<ISorting>());

        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
        Assert.False(request is IPagedQuery);
    }

    [Fact]
    public void CreateFilteringRequest()
    {
        var request = RequestBuilder.CreateFiltering(new Filter());

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IPagedQuery);
    }

    [Fact]
    public void CreatePagedRequest_Sorted()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>());

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreatePagedRequest_Sorted_Filtered()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>())
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreatePagedRequest_Filtered()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.False(request is ISortableQuery);
    }

    [Fact]
    public void CreatePagedRequest_Filtered_Sorted()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithFilter(new Filter())
            .WithSortable(Array.Empty<ISorting>());

        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateSortedRequest_Filtered()
    {
        var sortableQuery = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<ISortableQuery>(sortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(sortableQuery);
        Assert.False(sortableQuery is IPagedQuery);
    }

    [Fact]
    public void CreateSortedRequest_Filtered_Paged()
    {
        var sortableQuery = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithFilter(new Filter())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<ISortableQuery>(sortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(sortableQuery);
        Assert.IsAssignableFrom<IPagedQuery>(sortableQuery);
    }

    [Fact]
    public void CreateSortedRequest_Paged()
    {
        var sortableQuery = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<ISortableQuery>(sortableQuery);
        Assert.IsAssignableFrom<IPagedQuery>(sortableQuery);
        Assert.False(sortableQuery is IFilteringQuery);
    }

    [Fact]
    public void CreateSortedRequest_Paged_Filtered()
    {
        var sortableQuery = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithPaging(new Paging(0, 20))
            .WithFilter(new Filter());

        Assert.IsAssignableFrom<ISortableQuery>(sortableQuery);
        Assert.IsAssignableFrom<IPagedQuery>(sortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(sortableQuery);
    }

    [Fact]
    public void CreateFilteringRequest_Paged()
    {
        var request = RequestBuilder.CreateFiltering(new Filter())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
    }

    [Fact]
    public void CreateFilteringRequest_Paged_Sorted()
    {
        var request = RequestBuilder.CreateFiltering(new Filter())
            .WithPaging(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>());

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
    }

    [Fact]
    public void CreateFilteringRequest_Sorted()
    {
        var request = RequestBuilder.CreateFiltering(new Filter())
            .WithSortable(Array.Empty<ISorting>());

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IPagedQuery);
    }

    [Fact]
    public void CreateFilteringRequest_Sorted_Paged()
    {
        var request = RequestBuilder.CreateFiltering(new Filter())
            .WithSortable(Array.Empty<ISorting>())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<IFilteringQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
    }
}
