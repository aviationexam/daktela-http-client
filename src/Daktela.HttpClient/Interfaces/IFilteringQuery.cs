using Daktela.HttpClient.Api.Requests;

namespace Daktela.HttpClient.Interfaces;

public interface IFilteringQuery
{
    IFilter Filters { get; set; }
}
