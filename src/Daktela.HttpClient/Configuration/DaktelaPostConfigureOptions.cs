using Daktela.HttpClient.Api;
using Microsoft.Extensions.Options;

namespace Daktela.HttpClient.Configuration;

public class DaktelaPostConfigureOptions : IPostConfigureOptions<DaktelaOptions>
{
    public void PostConfigure(
        string? name, DaktelaOptions options
    ) => DaktelaJsonSerializerContext.SerializationDateTimeOffset = options.DateTimeOffset!.Value;
}
