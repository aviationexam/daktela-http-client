using Daktela.HttpClient.Api.Responses.Errors;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Responses;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class SingleResponse<T> where T : class
{
    [JsonPropertyName("error")]
    public IErrorResponse Error { get; set; } = null!;

    [JsonPropertyName("result")]
    public T Result { get; set; } = null!;

    [JsonPropertyName("_time")]
    public DateTimeOffset Time { get; set; }
}
