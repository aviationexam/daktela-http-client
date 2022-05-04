using Daktela.HttpClient.Attributes;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Api.Tickets;

public class Sla
{
    /// <summary>
    /// Name
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
    /// Low response
    /// </summary>
    [JsonPropertyName("response_low")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public int ResponseLow { get; set; }

    /// <summary>
    /// Normal response
    /// </summary>
    [JsonPropertyName("response_normal")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public int ResponseNormal { get; set; }

    /// <summary>
    /// High response
    /// </summary>
    [JsonPropertyName("response_high")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public int ResponseHigh { get; set; }

    /// <summary>
    /// Low solution
    /// </summary>
    [JsonPropertyName("solution_low")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public int SolutionLow { get; set; }

    /// <summary>
    /// Normal solution
    /// </summary>
    [JsonPropertyName("solution_normal")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public int SolutionNormal { get; set; }

    /// <summary>
    /// High solution
    /// </summary>
    [JsonPropertyName("solution_high")]
    [DaktelaRequirement(EOperation.Create | EOperation.Update)]
    public int SolutionHigh { get; set; }

    /// <summary>
    /// Unit
    /// </summary>
    [JsonPropertyName("unit")]
    public ETimeUnit Unit { get; set; }
}
