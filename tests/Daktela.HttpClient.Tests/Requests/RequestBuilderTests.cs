using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
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

        Assert.False(request is IFieldsQuery);
        Assert.False(request is IPagedQuery);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateEmptyRequest_Paged()
    {
        var request = RequestBuilder.CreateEmpty()
            .WithPaging(new Paging(0, 20));

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateEmptyRequest_Sorted()
    {
        var request = RequestBuilder.CreateEmpty()
            .WithSortable(Array.Empty<ISorting>());

        Assert.False(request is IFieldsQuery);
        Assert.False(request is IPagedQuery);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateEmptyRequest_Filtering()
    {
        var request = RequestBuilder.CreateEmpty()
            .WithFilter(CreateFilter);

        Assert.False(request is IFieldsQuery);
        Assert.False(request is IPagedQuery);
        Assert.False(request is ISortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateEmptyRequest_Fields()
    {
        var request = RequestBuilder.CreateEmpty()
            .WithFields(CreateFields);

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.False(request is IPagedQuery);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreatePagedRequest()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20));

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateSortedRequest()
    {
        var request = RequestBuilder.CreateSortable(Array.Empty<ISorting>());

        Assert.False(request is IFieldsQuery);
        Assert.False(request is IPagedQuery);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateFilteringRequest()
    {
        var request = RequestBuilder.CreateFiltering(CreateFilter);

        Assert.False(request is IFieldsQuery);
        Assert.False(request is IPagedQuery);
        Assert.False(request is ISortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateFieldsRequest()
    {
        var request = RequestBuilder.CreateFields(CreateFields);

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.False(request is IPagedQuery);
        Assert.False(request is ISortableQuery);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreatePagedRequest_Sorted()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>());

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreatePagedRequest_Sorted_Filtered()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>())
            .WithFilter(CreateFilter);

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreatePagedRequest_Filtered()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithFilter(CreateFilter);

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreatePagedRequest_Filtered_Fielded()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithFilter(CreateFilter);

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreatePagedRequest_Filtered_Sorted()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithFilter(CreateFilter)
            .WithSortable(Array.Empty<ISorting>());

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreatePagedRequest_Fielded()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithFields(CreateFields);

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreatePagedRequest_Sorted_Filtered_Fielded()
    {
        var request = RequestBuilder.CreatePaged(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>())
            .WithFilter(CreateFilter)
            .WithFields(CreateFields);

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateSortedRequest_Filtered()
    {
        var request = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithFilter(CreateFilter);

        Assert.False(request is IFieldsQuery);
        Assert.False(request is IPagedQuery);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateSortedRequest_Filtered_Paged()
    {
        var request = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithFilter(CreateFilter)
            .WithPaging(new Paging(0, 20));

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateSortedRequest_Paged()
    {
        var request = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithPaging(new Paging(0, 20));

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateSortedRequest_Paged_Filtered()
    {
        var request = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithPaging(new Paging(0, 20))
            .WithFilter(CreateFilter);

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateSortedRequest_Fielded()
    {
        var request = RequestBuilder.CreateSortable(Array.Empty<ISorting>())
            .WithFields(CreateFields);

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.False(request is IPagedQuery);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateFilteringRequest_Paged()
    {
        var request = RequestBuilder.CreateFiltering(CreateFilter)
            .WithPaging(new Paging(0, 20));

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateFilteringRequest_Paged_Sorted()
    {
        var request = RequestBuilder.CreateFiltering(CreateFilter)
            .WithPaging(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>());

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateFilteringRequest_Sorted()
    {
        var request = RequestBuilder.CreateFiltering(CreateFilter)
            .WithSortable(Array.Empty<ISorting>());

        Assert.False(request is IFieldsQuery);
        Assert.False(request is IPagedQuery);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateFilteringRequest_Sorted_Paged()
    {
        var request = RequestBuilder.CreateFiltering(CreateFilter)
            .WithSortable(Array.Empty<ISorting>())
            .WithPaging(new Paging(0, 20));

        Assert.False(request is IFieldsQuery);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateFilteringRequest_Fielded()
    {
        var request = RequestBuilder.CreateFiltering(CreateFilter)
            .WithFields(CreateFields);

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.False(request is IPagedQuery);
        Assert.False(request is ISortableQuery);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    [Fact]
    public void CreateFieldsRequest_Paged()
    {
        var request = RequestBuilder.CreateFields(CreateFields)
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.False(request is ISortableQuery);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateFieldsRequest_Paged_Sorted()
    {
        var request = RequestBuilder.CreateFields(CreateFields)
            .WithPaging(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>());

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateFieldsRequest_Sorted_Paged()
    {
        var request = RequestBuilder.CreateFields(CreateFields)
            .WithSortable(Array.Empty<ISorting>())
            .WithPaging(new Paging(0, 20));

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.False(request is IFilteringQuery);
    }

    [Fact]
    public void CreateFieldsRequest_Paged_Sorted_Filtered()
    {
        var request = RequestBuilder.CreateFields(CreateFields)
            .WithPaging(new Paging(0, 20))
            .WithSortable(Array.Empty<ISorting>())
            .WithFilter(CreateFilter);

        Assert.IsAssignableFrom<IFieldsQuery>(request);
        Assert.IsAssignableFrom<IPagedQuery>(request);
        Assert.IsAssignableFrom<ISortableQuery>(request);
        Assert.IsAssignableFrom<IFilteringQuery>(request);
    }

    private static IFilter CreateFilter => FilterBuilder<Contract>.WithValue(x => x.Field, EFilterOperator.Equal, "a value");

    private static IFields CreateFields => FieldBuilder<Contract>.Create(x => x.Field);

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    private class Contract
    {
        public string Field { get; } = null!;
    }
}
