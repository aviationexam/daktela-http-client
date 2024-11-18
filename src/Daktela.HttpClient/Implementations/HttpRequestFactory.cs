using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Web;

namespace Daktela.HttpClient.Implementations;

public class HttpRequestFactory(
    IContractValidation contractValidation,
    IOptions<DaktelaOptions> daktelaOptions
) : IHttpRequestFactory
{
    private readonly DaktelaOptions _daktelaOptions = daktelaOptions.Value;
    private readonly ESortDirectionEnumJsonConverter _sortDirectionEnumJsonConverter = new();
    private readonly EFilterOperatorEnumJsonConverter _filterOperatorEnumJsonConverter = new();

    public Uri CreateUri(
        string path
    ) => new(new Uri(_daktelaOptions.ApiDomain!, UriKind.Absolute), path);

    public HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, string path
    )
    {
        if (string.IsNullOrEmpty(_daktelaOptions.AccessToken))
        {
            throw new ArgumentException(
                $"The {nameof(DaktelaOptions)}.{nameof(_daktelaOptions.AccessToken)} is required"
            );
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
        HttpMethod method, string path, ICollection<KeyValuePair<string, string?>> queryParameters
    )
    {
        var uri = CreateUri(path);

        if (queryParameters.Count > 0)
        {
            var query = new QueryString(uri.Query);
            query = query.Add(QueryString.Create(queryParameters));

            var uriBuilder = new UriBuilder(uri)
            {
                Query = query.ToString(),
            };

            uri = uriBuilder.Uri;
        }

        return CreateHttpRequestMessage(method, uri);
    }

    public HttpRequestMessage CreateHttpRequestMessage(
        HttpMethod method, string path, IRequest request
    )
    {
        ICollection<KeyValuePair<string, string?>> queryDictionary = [];

        ApplyFields(request, queryDictionary);
        ApplySorting(request, queryDictionary);
        ApplyFilters(request, queryDictionary);
        ApplyPagination(request, queryDictionary);

        return CreateHttpRequestMessage(method, path, queryDictionary);
    }

    private void ApplyFields(IRequest request, ICollection<KeyValuePair<string, string?>> query)
    {
        if (request is IFieldsQuery { Fields.Items: { } fieldItems })
        {
            for (var i = 0; i < fieldItems.Count; i++)
            {
                var fieldItem = fieldItems.ElementAt(i);
                query.Add(KeyValuePair.Create<string, string?>($"fields[{i}]", fieldItem));
            }
        }
    }

    private void ApplySorting(IRequest request, ICollection<KeyValuePair<string, string?>> query)
    {
        if (request is ISortableQuery { Sorting: { } sorting })
        {
            for (var i = 0; i < sorting.Count; i++)
            {
                var sortItem = sorting.ElementAt(i);

                var sortDir = Encoding.UTF8.GetString(
                    _sortDirectionEnumJsonConverter.ToFirstEnumName(sortItem.Dir)
                );

                query.Add(KeyValuePair.Create<string, string?>($"sort[{i}][dir]", sortDir));
                query.Add(KeyValuePair.Create<string, string?>($"sort[{i}][field]", sortItem.Field));
            }
        }
    }

    private void ApplyFilters(IRequest request, ICollection<KeyValuePair<string, string?>> query)
    {
        if (request is IFilteringQuery { Filters: { } filters })
        {
            const string filterKey = "filter";

            SerializeFilters(filters, query, filterKey);
        }
    }

    private void SerializeFilters(IFilter filter, ICollection<KeyValuePair<string, string?>> query, string keyPrefix)
    {
        switch (filter)
        {
            case Filter coreFilter:
                var filterOperator = Encoding.UTF8.GetString(
                    _filterOperatorEnumJsonConverter.ToFirstEnumName(coreFilter.Operator)
                );

                query.Add(KeyValuePair.Create<string, string?>($"{keyPrefix}[field]", coreFilter.Field));
                query.Add(KeyValuePair.Create<string, string?>($"{keyPrefix}[operator]", filterOperator));
                query.Add(KeyValuePair.Create<string, string?>($"{keyPrefix}[value]", HttpUtility.HtmlEncode(coreFilter.Value)));

                if (!string.IsNullOrEmpty(coreFilter.Type))
                {
                    query.Add(KeyValuePair.Create<string, string?>($"{keyPrefix}[type]", coreFilter.Type));
                }

                break;
            case FilterGroup groupFilter:
                query.Add(KeyValuePair.Create<string, string?>($"{keyPrefix}[logic]", groupFilter.Logic == EFilterLogic.Or ? "or" : "and"));

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

    private void ApplyPagination(IRequest request, ICollection<KeyValuePair<string, string?>> query)
    {
        if (request is IPagedQuery { Paging: { } paging })
        {
            query.Add(KeyValuePair.Create<string, string?>("skip", paging.Skip.ToString()));
            query.Add(KeyValuePair.Create<string, string?>("take", paging.Take.ToString()));
        }
    }

    public HttpRequestMessage CreateHttpRequestMessage<
#pragma warning disable format
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicFields |
            DynamicallyAccessedMemberTypes.PublicProperties
        )]
        TBody
#pragma warning restore format
    >(
        IHttpRequestSerializer httpRequestSerializer,
        HttpMethod method,
        string path,
        TBody body,
        JsonTypeInfo<TBody> jsonTypeInfoForRequestType
    ) where TBody : class
    {
        var httpMessage = CreateHttpRequestMessage(method, path);

        var validationResult = contractValidation.Validate(body, method.Method switch
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

        httpMessage.Content = httpRequestSerializer.SerializeRequest(
            body,
            jsonTypeInfoForRequestType
        );

        return httpMessage;
    }
}
