using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Tests.Infrastructure;
using Daktela.HttpClient.Tests.Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Xunit;
using Xunit.Abstractions;

namespace Daktela.HttpClient.Tests.Integration;

public class IntegrationTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void EmptyInfrastructureTest()
    {
        using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var daktelaHttpClient = serviceProvider.GetRequiredService<IDaktelaHttpClient>();

        Assert.NotNull(daktelaHttpClient);
    }

    [Theory]
    [ManualInlineData("testing_user")]
    public async Task CreateContact(string name)
    {
        name = $"{name}_{DateTime.Now.Ticks}";
        _testOutputHelper.WriteLine($"Create user {name}");

        await using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var daktelaHttpClient = serviceProvider.GetRequiredService<IDaktelaHttpClient>();
        var httpRequestSerializer = serviceProvider.GetRequiredService<IHttpRequestSerializer>();

        var cancellationToken = CancellationToken.None;

        await daktelaHttpClient.PostAsync(
            httpRequestSerializer,
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            new CreateContact
            {
                Title = $"Title {name}",
                FirstName = null,
                LastName = null,
                Account = null,
                User = null,
                Description = null,
                CustomFields = null,
                Name = name
            },
            cancellationToken
        );

        var encodedName = HttpUtility.UrlEncode(name);

        await daktelaHttpClient.DeleteAsync($"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}", cancellationToken);
    }
}
