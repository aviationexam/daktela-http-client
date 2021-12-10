using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets
{
    public class TimeGroup
    {
        /// <summary>
        /// Name
        ///
        /// Unique name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; private set; } = null!;

        /// <summary>
        /// Title
        ///
        /// Formatted full name
        /// </summary>
        [JsonPropertyName("title")]
        [Required]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;
    }
}
