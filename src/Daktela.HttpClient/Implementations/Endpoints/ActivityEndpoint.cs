using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations.Endpoints;

public class ActivityEndpoint : IActivityEndpoint
{
    private readonly IDaktelaHttpClient _daktelaHttpClient;
    private readonly IHttpRequestSerializer _httpRequestSerializer;
    private readonly IHttpResponseParser _httpResponseParser;

    public ActivityEndpoint(
        IDaktelaHttpClient daktelaHttpClient,
        IHttpRequestSerializer httpRequestSerializer,
        IHttpResponseParser httpResponseParser
    )
    {
        _daktelaHttpClient = daktelaHttpClient;
        _httpRequestSerializer = httpRequestSerializer;
        _httpResponseParser = httpResponseParser;
    }

    public async Task<ReadActivity> CreateActivityAsync(
        CreateActivity activity, CancellationToken cancellationToken
    ) => await _daktelaHttpClient.PostAsync<CreateActivity, ReadActivity>(
        _httpRequestSerializer,
        _httpResponseParser,
        $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.UriPostfix}",
        activity,
        cancellationToken
    ).ConfigureAwait(false);
}
