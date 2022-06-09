using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Interfaces.Queries;

namespace Daktela.HttpClient.Interfaces.Requests.Builder;

public interface IWithFields<T> where T : class, IFieldsQuery
{
    T WithFields(IFields fields);
}
