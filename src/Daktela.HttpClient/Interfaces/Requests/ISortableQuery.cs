using Daktela.HttpClient.Api.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Interfaces.Requests;

public interface ISortableQuery : IRequest
{
    ICollection<Sorting> Sorting { get; }
}
