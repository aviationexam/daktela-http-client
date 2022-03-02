using Daktela.HttpClient.Api.Requests;

namespace Daktela.HttpClient.Interfaces;

public interface IPagedQuery
{
    Paging? Paging { get; set; }
}
