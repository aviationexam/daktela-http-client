using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Daktela.HttpClient.Api.Responses.Errors;

public class NestedErrorForm : ReadOnlyDictionary<string, IErrorForm>, IErrorForm
{
    public NestedErrorForm(IDictionary<string, IErrorForm> dictionary) : base(dictionary)
    {
    }
}
