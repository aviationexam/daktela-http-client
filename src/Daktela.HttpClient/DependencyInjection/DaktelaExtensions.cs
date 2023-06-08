using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Implementations.Endpoints;
using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daktela.HttpClient.Tests")]

namespace Daktela.HttpClient.DependencyInjection;

public static class DaktelaExtensions
{
    public static OptionsBuilder<DaktelaOptions> AddDaktelaHttpClient(
        this IServiceCollection serviceCollection,
        Action<DaktelaOptions> configure,
        Action<IHttpClientBuilder>? configureHttpClientBuilder = null
    )
    {
        var optionsBuilder = serviceCollection.AddOptions<DaktelaOptions>()
            .Configure(configure)
            .ValidateDataAnnotations();

        serviceCollection.AddDaktelaHttpClient(configureHttpClientBuilder);

        return optionsBuilder;
    }

    public static OptionsBuilder<DaktelaOptions> AddDaktelaHttpClient<TDependency>(
        this IServiceCollection serviceCollection,
        Action<DaktelaOptions, TDependency> configure,
        Action<IHttpClientBuilder>? configureHttpClientBuilder = null
    )
        where TDependency : class
    {
        var optionsBuilder = serviceCollection.AddOptions<DaktelaOptions>()
            .Configure(configure)
            .ValidateDataAnnotations();

        serviceCollection.AddDaktelaHttpClient(configureHttpClientBuilder);

        return optionsBuilder;
    }

    public static IServiceCollection AddDaktelaHttpClient(
        this IServiceCollection serviceCollection,
        Action<IHttpClientBuilder>? configureHttpClientBuilder = null
    )
    {
        serviceCollection
            .AddSingleton<IValidateOptions<DaktelaOptions>>(
                new DataAnnotationValidateOptions<DaktelaOptions>(nameof(DaktelaOptions))
            )
            .AddSingleton<IPostConfigureOptions<DaktelaOptions>, DaktelaPostConfigureOptions>();

        serviceCollection.TryAddSingleton<IHttpRequestFactory, HttpRequestFactory>();
        var httpClientBuilder = serviceCollection
            .AddHttpClient<IDaktelaHttpClient, DaktelaHttpClient>((serviceProvider, httpClient) =>
            {
                var daktelaOptions = serviceProvider.GetRequiredService<IOptions<DaktelaOptions>>().Value;

                httpClient.Timeout = daktelaOptions.Timeout!.Value;
                httpClient.BaseAddress = new Uri(daktelaOptions.ApiDomain!);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });

        configureHttpClientBuilder?.Invoke(httpClientBuilder);

        serviceCollection.TryAddSingleton(typeof(IPagedResponseProcessor<>), typeof(PagedResponseProcessor<>));

        serviceCollection.TryAddSingleton<IHttpResponseParser, HttpResponseParser>();
        serviceCollection.TryAddSingleton<IHttpRequestSerializer, HttpRequestSerializer>();

        serviceCollection.TryAddSingleton<IContractValidation, ContractValidation>();

        serviceCollection.TryAddScoped<IActivityEndpoint, ActivityEndpoint>();
        serviceCollection.TryAddScoped<IContactEndpoint, ContactEndpoint>();
        serviceCollection.TryAddScoped<IFileEndpoint, FileEndpoint>();
        serviceCollection.TryAddScoped<ITicketEndpoint, TicketEndpoint>();
        serviceCollection.TryAddScoped<ITicketsCategoryEndpoint, TicketsCategoryEndpoint>();

        return serviceCollection;
    }
}
