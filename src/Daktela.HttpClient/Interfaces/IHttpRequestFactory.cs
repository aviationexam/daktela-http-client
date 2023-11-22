using Daktela.HttpClient.Interfaces.Requests;
using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json.Serialization.Metadata;

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

    HttpRequestMessage CreateHttpRequestMessage<
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicFields |
            DynamicallyAccessedMemberTypes.PublicProperties
        )]
        TBody
    >(
        IHttpRequestSerializer httpRequestSerializer,
        HttpMethod method,
        string path,
        TBody body,
        JsonTypeInfo<TBody> jsonTypeInfoForRequestType
    ) where TBody : class;
}
