using Daktela.HttpClient.Tests.Infrastructure;
using Xunit;

namespace Daktela.HttpClient.Tests.Integration
{
    public class IntegrationTests
    {
        [Fact]
        public void EmptyInfrastructureTest()
        {
            using var daktelaHttpClient = TestHttpClientFactory.CreateHttpClient();
        }
    }
}
