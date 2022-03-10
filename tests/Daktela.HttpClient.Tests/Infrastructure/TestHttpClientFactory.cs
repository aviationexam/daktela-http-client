using Daktela.HttpClient.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Daktela.HttpClient.Tests.Infrastructure
{
    public static class TestHttpClientFactory
    {
        public static ServiceProvider CreateServiceProvider()
        {
            var configurationBuilder = new ConfigurationBuilder();

            var currentDirectory = Directory.GetCurrentDirectory();
            configurationBuilder.SetBasePath(currentDirectory);
            configurationBuilder.AddJsonFile("appsettings.json5", optional: false, reloadOnChange: false);
            configurationBuilder.AddJsonFile("user.appsettings.json5", optional: true, reloadOnChange: false);
            configurationBuilder.AddEnvironmentVariables(prefix: "DAKTELA_");

            var configuration = configurationBuilder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDaktelaHttpClient(c => configuration.Bind(c));

            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder
                    .AddFilter("System.Net.Http.HttpClient.IDaktelaHttpClient.RequestScope", LogLevel.Trace)
                    .AddFilter("System.Net.Http.HttpClient.IDaktelaHttpClient.RequestLogger", LogLevel.Trace);

                loggingBuilder
                    .AddDebug()
                    .AddConsole();
            });

            if (
                string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IS_AZURE_DEVOPS"))
                && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
            )
            {
                serviceCollection.Replace(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, HttpClientLoggingFilter>());
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
