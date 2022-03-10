using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net.Http;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class HttpRequestFactoryTests
{
    private const string DaktelaUrl = "https://daktela/api";
    private const string AccessToken = "my_secret_access_token";
    private readonly Uri _uri = new(DaktelaUrl);

    private readonly Mock<IOptions<DaktelaOptions>> _daktelaOptionsMock = new(MockBehavior.Strict);
    private readonly IHttpRequestFactory _httpRequestFactory;

    public HttpRequestFactoryTests()
    {
        _daktelaOptionsMock.Setup(x => x.Value)
            .Returns(new DaktelaOptions
            {
                AccessToken = AccessToken,
            });
        _httpRequestFactory = new HttpRequestFactory(_daktelaOptionsMock.Object);
    }

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
        Assert.Equal($"{DaktelaUrl}?skip=0&take=2&accessToken={AccessToken}", httpRequestMessage.RequestUri!.ToString());
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
        Assert.Equal($"{DaktelaUrl}?sort[0][dir]=asc&sort[0][field]=edited&accessToken={AccessToken}", httpRequestMessage.RequestUri!.ToString());
    }
}
