using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces;
using System;
using System.Net.Http;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class HttpRequestFactoryTests
{
    private const string DaktelaUrl = "https://daktela/api";
    private readonly Uri _uri = new(DaktelaUrl);

    private readonly IHttpRequestFactory _httpRequestFactory = new HttpRequestFactory();

    [Fact]
    public void PaginationWorks()
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Get,
            _uri,
            request: RequestBuilder.CreatePaged(new Paging(0, 2))
        );

        Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        Assert.NotNull(httpRequestMessage.RequestUri);
        Assert.Equal($"{DaktelaUrl}?skip=0&take=2", httpRequestMessage.RequestUri!.ToString());
    }

    [Fact]
    public void SortingWorks()
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Get,
            _uri,
            request: RequestBuilder.CreateSortable(new Sorting("edited", ESortDirection.Asc))
        );

        Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        Assert.NotNull(httpRequestMessage.RequestUri);
        Assert.Equal($"{DaktelaUrl}?sort[0][dir]=asc&sort[0][field]=edited", httpRequestMessage.RequestUri!.ToString());
    }
}
