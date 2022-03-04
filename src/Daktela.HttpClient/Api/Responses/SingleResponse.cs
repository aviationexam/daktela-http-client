using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Responses;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class SingleResponse<T> where T : class
{
    [JsonPropertyName("error")]
    public ICollection<string> Error { get; set; } = null!;

    [JsonPropertyName("result")]
    public T Result { get; set; } = null!;

    [JsonPropertyName("_time")]
    public DateTimeOffset Time { get; set; }
}
