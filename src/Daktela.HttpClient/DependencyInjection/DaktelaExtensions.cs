using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daktela.HttpClient.Tests")]

namespace Daktela.HttpClient.DependencyInjection;

public static class DaktelaExtensions
{
    public static IServiceCollection AddDaktelaHttpClient(
        this IServiceCollection serviceCollection,
        Action<DaktelaOptions>? configure = null
    )
    {
        if (configure != null)
        {
            serviceCollection.Configure(configure);
        }

        serviceCollection.AddHttpClient<IDaktelaHttpClient, DaktelaHttpClient>((serviceProvider, httpClient) =>
            {
                var daktelaOptions = serviceProvider.GetRequiredService<IOptions<DaktelaOptions>>().Value;

                httpClient.Timeout = daktelaOptions.Timeout;
                httpClient.BaseAddress = new Uri(daktelaOptions.BaseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });

        serviceCollection.AddSingleton<IHttpResponseParser, HttpResponseParser>();

        serviceCollection.AddScoped<IContactEndpoint, ContactEndpoint>();

        return serviceCollection;
    }
}
