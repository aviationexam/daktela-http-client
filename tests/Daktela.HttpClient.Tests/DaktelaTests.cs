using Daktela.HttpClient.Tests.Infrastructure;
using Xunit;

namespace Daktela.HttpClient.Tests
{
    public class DaktelaTests
    {
        [Fact]
        public void EmptyInfrastructureTest()
        {
            using var daktelaHttpClient = TestHttpClientFactory.CreateHttpClient();
        }
    }
}
