using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class RequestOptionBuilderTests
{
    [Fact]
    public void CreateEmptyRequest()
    {
        var requestOption = RequestOptionBuilder.CreateAutoPagingRequestOption(true);

        Assert.IsAssignableFrom<IAutoPagingRequestOption>(requestOption);
        Assert.True(requestOption.AutoPaging);
    }
}
