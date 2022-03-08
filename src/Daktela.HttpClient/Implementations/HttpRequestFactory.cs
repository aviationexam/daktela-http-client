using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Daktela.HttpClient.Implementations;

public class HttpRequestFactory : IHttpRequestFactory
{
    public HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, Uri uri
    ) => new(method, uri);

    public HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, Uri uri, IRequest request
    )
    {
        var queryDictionary = new NameValueCollection();

        ApplySorting(request, queryDictionary);
        ApplyFilters(request, queryDictionary);
        ApplyPagination(request, queryDictionary);

        if (queryDictionary.HasKeys())
        {
            var parsedQuery = HttpUtility.ParseQueryString(uri.Query);

            foreach (string key in queryDictionary.Keys)
            {
                parsedQuery[key] = queryDictionary[key];
            }

            var uriBuilder = new UriBuilder(uri)
            {
                Query = parsedQuery.ToString()
            };

            uri = uriBuilder.Uri;
        }

        return CreateHttpRequestMessage(method, uri);
    }

    private void ApplySorting(IRequest request, NameValueCollection query)
    {
        if (request is ISortableQuery { Sorting: { } sorting })
        {
            var sortDirectionSerializer = new EnumsConverter<ESortDirection>();

            for (var i = 0; i < sorting.Count; i++)
            {
                var sortItem = sorting.ElementAt(i);
                query.Add($"sort[{i}][dir]", sortDirectionSerializer.ReverseMapping[sortItem.Dir]);
                query.Add($"sort[{i}][field]", sortItem.Field);
            }
        }
    }

    private void ApplyFilters(IRequest request, NameValueCollection query)
    {
        if (request is IFilteringQuery { Filters: { } filters })
        {
            // TODO filters
        }
    }

    private void ApplyPagination(IRequest request, NameValueCollection query)
    {
        if (request is IPagedQuery { Paging: { } paging })
        {
            query.Add("skip", paging.Skip.ToString());
            query.Add("take", paging.Take.ToString());
        }
    }

    public HttpRequestMessage CreateHttpRequestMessage<TBody>(
        HttpMethod method, Uri uri, TBody? body
    ) where TBody : class
    {
        var httpMessage = CreateHttpRequestMessage(method, uri);

        return httpMessage;
    }
}
