using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces.Requests.Options;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
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
