using Daktela.HttpClient.Attributes;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Files;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/working-with/tickets#create-ticket-with-comment-and-attachment">Object</a> represents File structure.
/// </summary>
public class File
{
    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Title
    /// </summary>
    [JsonPropertyName("title")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Byte length
    /// </summary>
    [JsonPropertyName("size")]
    [DaktelaNonZeroValue(EOperation.Create | EOperation.Update)]

    public long Size { get; set; }

    /// <summary>
    /// MIME type
    /// </summary>
    [JsonPropertyName("type")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string? Type { get; set; }
}
