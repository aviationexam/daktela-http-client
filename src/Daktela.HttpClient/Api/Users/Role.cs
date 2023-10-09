using Daktela.HttpClient.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Users;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/roles">Object</a> accesses returns all information about user roles - name role, title and relation objects users, rights.
/// </summary>
public class Role
{
    /// <summary>
    /// Name
    ///
    /// Unique name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Title
    /// </summary>
    [JsonPropertyName("title")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Shortcuts
    /// </summary>
    [JsonPropertyName("shortcuts")]
    public IReadOnlyCollection<object> Shortcuts { get; set; } = null!;

    /// <summary>
    /// Options
    /// </summary>
    [JsonPropertyName("options")]
    public IReadOnlyDictionary<string, object> Options { get; set; } = null!;
}
