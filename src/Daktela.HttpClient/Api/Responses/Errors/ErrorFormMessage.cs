namespace Daktela.HttpClient.Api.Responses.Errors;

public class ErrorFormMessage : IErrorForm
{
    public string ErrorMessage { get; set; } = null!;
}
