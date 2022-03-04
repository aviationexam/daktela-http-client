using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Interfaces.Queries;

public interface ISortableQuery : IRequest
{
    IReadOnlyCollection<Sorting> Sorting { get; }
}
