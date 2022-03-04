using Daktela.HttpClient.Implementations.ResponseBehaviours;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;

namespace Daktela.HttpClient.Implementations;

public static class ResponseBehaviourBuilder
{
    public static IResponseBehaviour CreateEmpty() => new EmptyResponseBehaviour();
}
