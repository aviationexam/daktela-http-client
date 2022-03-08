namespace Daktela.HttpClient.Api.Requests;

public record Sorting(string Field, ESortDirection Dir) : ISorting;
