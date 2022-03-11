using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Responses.Errors;

public class ComplexErrorResponse : IErrorResponse
{
    [JsonPropertyName("primary")]
    public ICollection<string>? Primary { get; set; }

    [JsonPropertyName("form")]
    public IDictionary<string, IErrorForm>? Form { get; set; }
}
