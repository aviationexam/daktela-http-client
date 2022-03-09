using System;
using System.Net;

namespace Daktela.HttpClient.Exceptions;

public class UnexpectedHttpResponseException : Exception
{
    public string Uri { get; }

    public HttpStatusCode StatusCode { get; }

    public string HttpContent { get; }

    public UnexpectedHttpResponseException(string uri, HttpStatusCode statusCode, string httpContent)
    {
        Uri = uri;
        StatusCode = statusCode;
        HttpContent = httpContent;
    }
}
