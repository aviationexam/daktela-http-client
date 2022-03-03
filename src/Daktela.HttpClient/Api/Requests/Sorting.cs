using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Requests;

public class Sorting
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = null!;

    [JsonPropertyName("dir")]
    public ESortDirection Dir { get; set; }
}
