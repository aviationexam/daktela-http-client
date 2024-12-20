using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class HttpRequestFactoryTests
{
    private const string DaktelaUrl = "https://daktela";
    private const string DaktelaContractPath = "api/contract";
    private const string AccessToken = "my_secret_access_token";

    private readonly Mock<IContractValidation> _contractValidationMock = new(MockBehavior.Strict);
    private readonly Mock<IOptions<DaktelaOptions>> _daktelaOptionsMock = new(MockBehavior.Strict);
    private readonly IHttpRequestFactory _httpRequestFactory;

    public HttpRequestFactoryTests()
    {
        _daktelaOptionsMock.Setup(x => x.Value)
            .Returns(new DaktelaOptions
            {
                ApiDomain = DaktelaUrl,
                AccessToken = AccessToken,
            });
        _httpRequestFactory = new HttpRequestFactory(
            _contractValidationMock.Object,
            _daktelaOptionsMock.Object
        );
    }

    [Fact]
    public void FieldingWorks()
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Get,
            DaktelaContractPath,
            request: RequestBuilder.CreateFields(new Fields(["name"]))
        );

        Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        Assert.NotNull(httpRequestMessage.RequestUri);
#if NET9_0_OR_GREATER
        Assert.Equal($"{DaktelaUrl}/{DaktelaContractPath}?fields%5b0%5d=name&accessToken={AccessToken}", httpRequestMessage.RequestUri!.ToString());
#else
        Assert.Equal($"{DaktelaUrl}/{DaktelaContractPath}?fields[0]=name&accessToken={AccessToken}", httpRequestMessage.RequestUri!.ToString());
#endif
    }

    [Fact]
    public void PaginationWorks()
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Get,
            DaktelaContractPath,
            request: RequestBuilder.CreatePaged(new Paging(0, 2))
        );

        Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        Assert.NotNull(httpRequestMessage.RequestUri);
        Assert.Equal($"{DaktelaUrl}/{DaktelaContractPath}?skip=0&take=2&accessToken={AccessToken}", httpRequestMessage.RequestUri!.ToString());
    }

    [Fact]
    public void SortingWorks()
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Get,
            DaktelaContractPath,
            request: RequestBuilder.CreateSortable(new Sorting("edited", ESortDirection.Asc))
        );

        Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        Assert.NotNull(httpRequestMessage.RequestUri);
#if NET9_0_OR_GREATER
        Assert.Equal($"{DaktelaUrl}/{DaktelaContractPath}?sort%5b0%5d%5bdir%5d=asc&sort%5b0%5d%5bfield%5d=edited&accessToken={AccessToken}", httpRequestMessage.RequestUri!.ToString());
#else
        Assert.Equal($"{DaktelaUrl}/{DaktelaContractPath}?sort[0][dir]=asc&sort[0][field]=edited&accessToken={AccessToken}", httpRequestMessage.RequestUri!.ToString());
#endif
    }

    [Fact]
    public void FilteringWorks()
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Get,
            DaktelaContractPath,
            request: RequestBuilder.CreateFiltering(new Filter("contact", EFilterOperator.Equal, "a value"))
        );

        Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        Assert.NotNull(httpRequestMessage.RequestUri);
#if NET9_0_OR_GREATER
        Assert.Equal(
            $"{DaktelaUrl}/{DaktelaContractPath}?filter%5bfield%5d=contact&filter%5boperator%5d=eq&filter%5bvalue%5d=a+value&accessToken={AccessToken}",
            httpRequestMessage.RequestUri!.ToString()
        );
#else
        Assert.Equal(
            $"{DaktelaUrl}/{DaktelaContractPath}?filter[field]=contact&filter[operator]=eq&filter[value]=a+value&accessToken={AccessToken}",
            httpRequestMessage.RequestUri!.ToString()
        );
#endif
    }

    [Fact]
    public void FilteringEscapedWorks()
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Get,
            DaktelaContractPath,
            request: RequestBuilder.CreateFiltering(new Filter("contact", EFilterOperator.Equal, "a+value"))
        );

        Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        Assert.NotNull(httpRequestMessage.RequestUri);
#if NET9_0_OR_GREATER
        Assert.Equal(
            $"{DaktelaUrl}/{DaktelaContractPath}?filter%5bfield%5d=contact&filter%5boperator%5d=eq&filter%5bvalue%5d=a%2bvalue&accessToken={AccessToken}",
            httpRequestMessage.RequestUri!.ToString()
        );
#else
        Assert.Equal(
            $"{DaktelaUrl}/{DaktelaContractPath}?filter[field]=contact&filter[operator]=eq&filter[value]=a%2bvalue&accessToken={AccessToken}",
            httpRequestMessage.RequestUri!.ToString()
        );
#endif
    }

    [Fact]
    public void ComplexFilteringWorks()
    {
        using var httpRequestMessage = _httpRequestFactory.CreateHttpRequestMessage(
            HttpMethod.Get,
            DaktelaContractPath,
            request: RequestBuilder.CreateFiltering(new FilterGroup(EFilterLogic.And, [
                new Filter("name", EFilterOperator.Equal, "Johan"),
                new Filter("email", EFilterOperator.EndsWith, "@gmail.com"),
            ]))
        );

        Assert.Equal(HttpMethod.Get, httpRequestMessage.Method);
        Assert.NotNull(httpRequestMessage.RequestUri);
#if NET9_0_OR_GREATER
        Assert.Equal(
            $"{DaktelaUrl}/{DaktelaContractPath}?filter%5blogic%5d=and"
            + "&filter%5bfilters%5d%5b0%5d%5bfield%5d=name&filter%5bfilters%5d%5b0%5d%5boperator%5d=eq&filter%5bfilters%5d%5b0%5d%5bvalue%5d=Johan"
            + "&filter%5bfilters%5d%5b1%5d%5bfield%5d=email&filter%5bfilters%5d%5b1%5d%5boperator%5d=endswith&filter%5bfilters%5d%5b1%5d%5bvalue%5d=%40gmail.com"
            + $"&accessToken={AccessToken}",
            httpRequestMessage.RequestUri!.ToString()
        );
#else
        Assert.Equal(
            $"{DaktelaUrl}/{DaktelaContractPath}?filter[logic]=and"
            + "&filter[filters][0][field]=name&filter[filters][0][operator]=eq&filter[filters][0][value]=Johan"
            + "&filter[filters][1][field]=email&filter[filters][1][operator]=endswith&filter[filters][1][value]=%40gmail.com"
            + $"&accessToken={AccessToken}",
            httpRequestMessage.RequestUri!.ToString()
        );
#endif
    }
}
