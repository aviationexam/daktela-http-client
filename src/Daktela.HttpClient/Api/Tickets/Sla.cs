using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class Sla
{
    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Title
    /// </summary>
    [JsonPropertyName("title")]
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Low response
    /// </summary>
    [JsonPropertyName("response_low")]
    [Required]
    public int ResponseLow { get; set; }

    /// <summary>
    /// Normal response
    /// </summary>
    [JsonPropertyName("response_normal")]
    [Required]
    public int ResponseNormal { get; set; }

    /// <summary>
    /// High response
    /// </summary>
    [JsonPropertyName("response_high")]
    [Required]
    public int ResponseHigh { get; set; }

    /// <summary>
    /// Low solution
    /// </summary>
    [JsonPropertyName("solution_low")]
    [Required]
    public int SolutionLow { get; set; }

    /// <summary>
    /// Normal solution
    /// </summary>
    [JsonPropertyName("solution_normal")]
    [Required]
    public int SolutionNormal { get; set; }

    /// <summary>
    /// High solution
    /// </summary>
    [JsonPropertyName("solution_high")]
    [Required]
    public int SolutionHigh { get; set; }

    /// <summary>
    /// Unit
    /// </summary>
    [JsonPropertyName("unit")]
    public ETimeUnit Unit { get; set; }
}
