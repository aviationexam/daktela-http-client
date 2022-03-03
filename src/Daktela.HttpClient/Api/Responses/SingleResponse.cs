using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Responses;

// ReSharper disable once ClassNeverInstantiated.Global
public class SingleResponse<T> where T : class
{
    [JsonPropertyName("error")]
    public ICollection<string> Error { get; set; } = null!;

    [JsonPropertyName("result")]
    public T Result { get; set; } = null!;

    [JsonPropertyName("_time")]
    public DateTimeOffset Time { get; set; }
}
