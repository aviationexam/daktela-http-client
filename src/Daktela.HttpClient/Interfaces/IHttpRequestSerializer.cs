using System.Net.Http;

namespace Daktela.HttpClient.Interfaces;

public interface IHttpRequestSerializer
{
    HttpContent SerializeRequest<TContract>(
        TContract contract
    ) where TContract : class;
}
