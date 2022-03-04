using Daktela.HttpClient.Api.Requests;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface IFilteringQuery : IRequest
{
    IFilter Filters { get; }
}
