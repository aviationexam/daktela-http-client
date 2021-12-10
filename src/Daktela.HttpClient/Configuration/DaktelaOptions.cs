using System;

namespace Daktela.HttpClient.Configuration
{
    public class DaktelaOptions
    {
        public string BaseUrl { get; set; } = null!;

        public string AccessToken { get; set; } = null!;

        public TimeSpan Timeout { get; set; }
    }
}
