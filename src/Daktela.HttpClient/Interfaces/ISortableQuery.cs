using Daktela.HttpClient.Api.Requests;
using System.Collections.Generic;

namespace Daktela.HttpClient.Interfaces;

public interface ISortableQuery
{
    ICollection<Sorting> Sorting { get; set; }
}
