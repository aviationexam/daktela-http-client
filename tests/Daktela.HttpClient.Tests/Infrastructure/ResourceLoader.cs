using Daktela.HttpClient.Tests.Endpoints;
using System;
using System.IO;
using System.Reflection;

namespace Daktela.HttpClient.Tests.Infrastructure;

public static class ResourceLoader
{
    public static Stream LoadEmbeddedJson(this string name)
    {
        var resourceName = $"Daktela.HttpClient.Tests.json_responses.{name}.json";
        var assembly = typeof(DaktelaHttpClientMock).GetTypeInfo().Assembly;
        return assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException($"Resource {resourceName} was not found");
    }
}
