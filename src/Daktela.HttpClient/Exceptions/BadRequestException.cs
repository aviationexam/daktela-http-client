using Daktela.HttpClient.Api.Responses.Errors;

namespace Daktela.HttpClient.Exceptions;

public class BadRequestException : DaktelaException
{
    public IErrorResponse ErrorsResponse { get; }

    protected BadRequestException(IErrorResponse errorsResponse)
    {
        ErrorsResponse = errorsResponse;
    }
}

public class BadRequestException<TContract> : BadRequestException
    where TContract : class
{
    public TContract Contract { get; }

    public BadRequestException(TContract contract, IErrorResponse errorsResponse) : base(errorsResponse)
    {
        Contract = contract;
    }
}
