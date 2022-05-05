using System.Collections.Generic;

namespace Daktela.HttpClient.Api.CustomFields;

public class CustomFields : Dictionary<string, ICollection<string>>, ICustomFields
{
}
