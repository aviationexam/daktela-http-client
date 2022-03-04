using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using Xunit;

namespace Daktela.HttpClient.Tests.ResponseBehaviours;

public class ResponseBuilderTests
{
    [Fact]
    public void CreateEmptyRequest()
    {
        var responseMetadata = ResponseBehaviourBuilder.CreateEmpty();

        Assert.IsAssignableFrom<IResponseBehaviour>(responseMetadata);
    }
}
