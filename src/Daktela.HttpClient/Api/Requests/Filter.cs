namespace Daktela.HttpClient.Api.Requests;

public record Filter(
    string Field,
    EFilterOperator Operator,
    string Value,
    string? Type = null
) : IFilter;
