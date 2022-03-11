namespace Daktela.HttpClient.Api.Responses.Errors;

public class NestedErrorForm : IErrorForm
{
    public IErrorForm InnerError { get; set; } = null!;
}
