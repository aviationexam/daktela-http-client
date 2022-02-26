using Daktela.HttpClient.Interfaces;

namespace Daktela.HttpClient.Implementations;

public class DaktelaHttpClient : IDaktelaHttpClient
{
    private readonly System.Net.Http.HttpClient _httpClient;

    public DaktelaHttpClient(
        System.Net.Http.HttpClient httpClient
    )
    {
        _httpClient = httpClient;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
