using System.Collections.Generic;

namespace Daktela.HttpClient.Api.Responses.Errors;

public class NestedErrorForm : Dictionary<string, IErrorForm>, IErrorForm
{
    public NestedErrorForm(
        IReadOnlyDictionary<string, IErrorForm> dictionary
    ) : base(dictionary)
    {
    }
}
