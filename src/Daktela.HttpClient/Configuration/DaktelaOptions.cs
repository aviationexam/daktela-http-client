using System;
using System.ComponentModel.DataAnnotations;

namespace Daktela.HttpClient.Configuration;

public class DaktelaOptions
{
    [Required]
    public string? ApiDomain { get; set; }

    [Required]
    public string? AccessToken { get; set; }

    /// <summary>
    /// HTTP request timeout
    /// </summary>
    [Required]
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Remote server date time timezone
    /// </summary>
    public string? DateTimeTimezone { get; set; }

    private TimeSpan? _dateTimeOffset;

    /// <summary>
    /// Remote server date time offset
    /// </summary>
    [Required]
    public TimeSpan? DateTimeOffset
    {
        get
        {
            if (!string.IsNullOrEmpty(DateTimeTimezone))
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(DateTimeTimezone);

                return timeZoneInfo.GetUtcOffset(System.DateTimeOffset.Now);
            }

            return _dateTimeOffset;
        }
        set => _dateTimeOffset = value;
    }
}
