using System.Collections.Generic;

namespace Daktela.HttpClient.Api.CustomFields;

public interface ICustomFields : IDictionary<string, ICollection<string>>
{
}
