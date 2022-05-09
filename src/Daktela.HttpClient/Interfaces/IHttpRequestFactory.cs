using Daktela.HttpClient.Interfaces.Requests;
using System;
using System.Collections.Specialized;
using System.Net.Http;

namespace Daktela.HttpClient.Interfaces;

public interface IHttpRequestFactory
{
    Uri CreateUri(string path);

    HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, string path
    );

    HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, string path, NameValueCollection queryParameters
    );

    HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, string path, IRequest request
    );

    HttpRequestMessage CreateHttpRequestMessage<TBody>(
        IHttpRequestSerializer httpRequestSerializer,
        HttpMethod method,
        string path,
        TBody body
    ) where TBody : class;
}
