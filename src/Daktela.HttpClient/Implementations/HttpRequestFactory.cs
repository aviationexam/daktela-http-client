using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Daktela.HttpClient.Implementations;

public class HttpRequestFactory : IHttpRequestFactory
{
    private readonly IContractValidation _contractValidation;
    private readonly DaktelaOptions _daktelaOptions;

    private readonly EnumsConverter<ESortDirection> _sortDirectionSerializer = new();
    private readonly EnumsConverter<EFilterOperator> _filterDirectionSerializer = new();

    public HttpRequestFactory(
        IContractValidation contractValidation,
        IOptions<DaktelaOptions> daktelaOptions
    )
    {
        _contractValidation = contractValidation;
        _daktelaOptions = daktelaOptions.Value;
    }

    public Uri CreateUri(
        string path
    ) => new(new Uri(_daktelaOptions.ApiDomain!, UriKind.Absolute), path);

    public HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, string path
    )
    {
        if (string.IsNullOrEmpty(_daktelaOptions.AccessToken))
        {
            throw new ArgumentException($"The {nameof(DaktelaOptions)}.{nameof(_daktelaOptions.AccessToken)} is required");
        }

        var uri = CreateUri(path);

        return CreateHttpRequestMessage(method, uri);
    }

    private HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, Uri uri
    )
    {
        var parsedQuery = HttpUtility.ParseQueryString(uri.Query);

        parsedQuery.Add("accessToken", _daktelaOptions.AccessToken);

        var uriBuilder = new UriBuilder(uri)
        {
            Query = parsedQuery.ToString()
        };

        uri = uriBuilder.Uri;

        return new HttpRequestMessage(method, uri);
    }

    public HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, string path, IRequest request
    )
    {
        var uri = CreateUri(path);

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
            for (var i = 0; i < sorting.Count; i++)
            {
                var sortItem = sorting.ElementAt(i);
                query.Add($"sort[{i}][dir]", _sortDirectionSerializer.ReverseMapping[sortItem.Dir]);
                query.Add($"sort[{i}][field]", sortItem.Field);
            }
        }
    }

    private void ApplyFilters(IRequest request, NameValueCollection query)
    {
        if (request is IFilteringQuery { Filters: { } filters })
        {
            const string filterKey = "filter";

            SerializeFilters(filters, query, filterKey);
        }
    }

    private void SerializeFilters(IFilter filter, NameValueCollection query, string keyPrefix)
    {
        switch (filter)
        {
            case Filter coreFilter:
                var operatorType = _filterDirectionSerializer.ReverseMapping[coreFilter.Operator];

                query.Add($"{keyPrefix}[field]", coreFilter.Field);
                query.Add($"{keyPrefix}[operator]", operatorType);
                query.Add($"{keyPrefix}[value]", HttpUtility.HtmlEncode(coreFilter.Value));

                if (!string.IsNullOrEmpty(coreFilter.Type))
                {
                    query.Add($"{keyPrefix}[type]", coreFilter.Type);
                }

                break;
            case FilterGroup groupFilter:
                query.Add($"{keyPrefix}[logic]", groupFilter.Logic == EFilterLogic.Or ? "or" : "and");

                for (var i = 0; i < groupFilter.Filters.Count; i++)
                {
                    var innerFilter = groupFilter.Filters.ElementAt(i);

                    SerializeFilters(innerFilter, query, $"{keyPrefix}[filters][{i}]");
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
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
        IHttpRequestSerializer httpRequestSerializer,
        HttpMethod method,
        string path,
        TBody body
    ) where TBody : class
    {
        var httpMessage = CreateHttpRequestMessage(method, path);

        var validationResult = _contractValidation.Validate(body, method.Method switch
        {
            "GET" => EOperation.Read,
            "POST" => EOperation.Create,
            "PUT" => EOperation.Update,
            "DELETE" => EOperation.Delete,
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null),
        });

        if (validationResult != ValidationResult.Success)
        {
            throw new ValidationException(validationResult!, null, body);
        }

        httpMessage.Content = httpRequestSerializer.SerializeRequest(body);

        return httpMessage;
    }
}
