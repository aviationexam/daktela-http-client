using Daktela.HttpClient.Api.Files;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class MergeEndpoint : IMergeEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpRequestFactory _httpRequestFactory;
    private readonly EFileSourceEnumJsonConverter _fileSourceEnumJsonConverter = new();

    public MergeEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpRequestFactory httpRequestFactory
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpRequestFactory = httpRequestFactory;
    }
}
