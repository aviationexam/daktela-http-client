using System.Collections.Generic;

namespace Daktela.HttpClient.Api.Responses.Errors;

public class ErrorFormMessages : IErrorForm
{
    public IReadOnlyCollection<string> ErrorMessages { get; set; } = null!;
}
