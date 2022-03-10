using Daktela.HttpClient.Tests.Infrastructure;
using Xunit;

namespace Daktela.HttpClient.Tests.Integration;

public class IntegrationTests
{
    [Fact]
    public void EmptyInfrastructureTest()
    {
        using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var daktelaHttpClient = serviceProvider.GetRequiredService<IDaktelaHttpClient>();

        Assert.NotNull(daktelaHttpClient);
    }
}
