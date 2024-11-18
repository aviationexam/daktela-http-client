using System.Collections.ObjectModel;

namespace Daktela.HttpClient.Api.Responses.Errors;

public class PlainErrorResponse : Collection<string>, IErrorResponse;
