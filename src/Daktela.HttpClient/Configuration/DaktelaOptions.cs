using System;

namespace Daktela.HttpClient.Configuration;

public class DaktelaOptions
{
    public string BaseUrl { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// HTTP request timeout
    /// </summary>
    public TimeSpan Timeout { get; set; }

    /// <summary>
    /// Remote server date time offset
    /// </summary>
    public TimeSpan DateTimeOffset { get; set; }
}
