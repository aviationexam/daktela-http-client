using System.Collections.Generic;

namespace Daktela.HttpClient.Api.CustomFields;

public class CustomFields : Dictionary<string, IReadOnlyCollection<string>>, ICustomFields
{
    public CustomFields()
    {
    }

    public CustomFields(
        IReadOnlyDictionary<string, IReadOnlyCollection<string>> dictionary
    ) : base(dictionary)
    {
    }
}
