using Daktela.HttpClient.Implementations.Requests.Options;
using Daktela.HttpClient.Interfaces.Requests.Options;

namespace Daktela.HttpClient.Implementations;

public static class RequestOptionBuilder
{
    public static IAutoPagingRequestOption CreateAutoPagingRequestOption(bool autoPaging) => new AutoPagingRequestOption(autoPaging);
}
