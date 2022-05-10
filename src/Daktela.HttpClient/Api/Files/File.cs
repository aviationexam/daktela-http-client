using Daktela.HttpClient.Attributes;
using Daktela.HttpClient.Interfaces.Endpoints;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Files;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/working-with/tickets#create-ticket-with-comment-and-attachment">Object</a> represents File structure.
/// </summary>
public class File
{
    /// <summary>
    /// Name
    ///
    /// Response from the <see cref="IFileEndpoint.UploadFileAsync"/>
    /// </summary>
    [JsonPropertyName("name")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string FileIdentifier { get; set; } = null!;

    /// <summary>
    /// Title
    ///
    /// FileName of the uploaded file
    /// </summary>
    [JsonPropertyName("title")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string FileName { get; set; } = null!;

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
