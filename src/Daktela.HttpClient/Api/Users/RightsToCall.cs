using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Users;

/// <summary>
/// <a href="https://www.daktela.com/apihelp/v6/models/rightstocall">RightsToCall</a>
/// </summary>
public class RightsToCall
{
    /// <summary>
    /// name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Title
    /// </summary>
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Time
    /// </summary>
    [JsonPropertyName("time")]
    public string Time { get; set; } = null!;

    /// <summary>
    /// Rules
    ///
    /// A Dial Pattern is a unique set of digits that will select this trunk.
    ///
    /// <ul>
    /// Rules:
    /// <li><b>X</b> matches any digit from 0-9</li>
    /// <li><b>Z</b> matches any digit from 1-9</li>
    /// <li><b>N</b> matches any digit from 2-9</li>
    /// <li><b>[1237-9]</b> matches any digit or letter in the brackets (in this example, 1,2,3,7,8,9)</li>
    /// <li><b>.</b> wildcard, matches one or more characters</li>
    /// <li><b>|</b> separates a dialing prefix from the number (for example, 9|NXXXXXX would match when some dialed "95551234" but would only pass "5551234" to the trunks)</li>
    /// </ul>
    /// </summary>
    [JsonPropertyName("rules")]
    public ICollection<string> Rules { get; set; } = null!;
}
