using System;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/activities-attachments">Object</a> represents Attachment of an Activity. One attachment have single activity, one activity can have many attachments.
/// </summary>
public class ReadActivityAttachment
{
    /// <summary>
    /// Activities
    /// </summary>
    [JsonPropertyName("activity")]
    public ReadActivity? Activity { get; set; }

    /// <summary>
    /// Is inline
    /// </summary>
    [JsonPropertyName("inline")]
    public string Inline { get; set; } = null!;

    /// <summary>
    /// CID
    /// </summary>
    [JsonPropertyName("cid")]
    public string Cid { get; set; } = null!;

    /// <summary>
    /// Title
    ///
    /// FileName of the uploaded file
    /// </summary>
    [JsonPropertyName("title")]
    public string FileName { get; set; } = null!;

    /// <summary>
    /// MIME type
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Byte length
    /// </summary>
    [JsonPropertyName("size")]
    public long Size { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    [JsonPropertyName("time")]
    public DateTimeOffset UploadTime { get; set; }

    /// <summary>
    /// Undocumented property
    /// </summary>
    [JsonPropertyName("file")]
    public long? FileId { get; set; }
}
