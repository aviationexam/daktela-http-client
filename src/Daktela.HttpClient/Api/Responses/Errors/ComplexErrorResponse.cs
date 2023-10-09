using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Responses.Errors;

public class ComplexErrorResponse : IErrorResponse
{
    [JsonPropertyName("primary")]
    public IReadOnlyCollection<string>? Primary { get; set; }

    [JsonPropertyName("form")]
    public IReadOnlyDictionary<string, IErrorForm>? Form { get; set; }
}
