using Daktela.HttpClient.Interfaces.Requests;
using System;
using System.Collections.Specialized;
using System.Net.Http;

namespace Daktela.HttpClient.Interfaces;

public interface IHttpRequestFactory
{
    HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, Uri uri
    );

    HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, Uri uri, IRequest request
    );

    HttpRequestMessage CreateHttpRequestMessage<TBody>(
        HttpMethod method, Uri uri, TBody? body
    ) where TBody : class;
}
