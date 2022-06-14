using System;
using System.Text.Json;

namespace Daktela.HttpClient.Exceptions;

public class JsonDaktelaException : DaktelaException
{
    public JsonException Exception { get; }

    public Uri? RequestUri { get; }

    public JsonDaktelaException(JsonException exception, Uri? requestUri) : base(exception.Message)
    {
        Exception = exception;
        RequestUri = requestUri;
    }
}
