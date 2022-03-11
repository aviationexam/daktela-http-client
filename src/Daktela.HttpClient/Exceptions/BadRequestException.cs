using Daktela.HttpClient.Api.Responses.Errors;

namespace Daktela.HttpClient.Exceptions;

public class BadRequestException<TContract> : DaktelaException
    where TContract : class
{
    public TContract Contract { get; }

    public IErrorResponse ErrorsResponse { get; }

    public BadRequestException(TContract contract, IErrorResponse errorsResponse)
    {
        Contract = contract;
        ErrorsResponse = errorsResponse;
    }
}
