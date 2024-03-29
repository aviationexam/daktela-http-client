using Daktela.HttpClient.Api.Responses.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Responses;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class ListResponse<T> where T : class
{
    [JsonPropertyName("error")]
    public IErrorResponse Error { get; set; } = null!;

    [JsonPropertyName("result")]
    public ResultObject Result { get; set; } = null!;

    [JsonPropertyName("_time")]
    public DateTimeOffset Time { get; set; }

    public class ResultObject
    {
        [JsonPropertyName("data")]
        public ICollection<T> Data { get; set; } = null!;

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
