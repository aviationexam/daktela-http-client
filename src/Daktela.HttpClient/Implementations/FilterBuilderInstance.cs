using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Requests;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json;

namespace Daktela.HttpClient.Implementations;

public class FilterBuilderInstance<TContract>
    where TContract : class
{
    public IFilter WithValue<T>(
        Expression<Func<TContract, T>> propertySelector, EFilterOperator filterOperator, string value, string? type = null
    ) => new Filter(PathBuilder<TContract>.Build(propertySelector), filterOperator, value, type);

    public IFilter WithEnumValue<T, TEnum>(
        Expression<Func<TContract, T>> propertySelector, EFilterOperator filterOperator, TEnum enumValue, string? type = null
    ) where TEnum : struct, Enum
    {
        var serializedEnum = JsonSerializer.Serialize(enumValue, DaktelaJsonSerializerContext.Default.Options);

        return new Filter(
            PathBuilder<TContract>.Build(propertySelector), filterOperator,
            serializedEnum,
            type
        );
    }

    public IFilter WithGroupOfValue(
        EFilterLogic filterLogic,
        IReadOnlyCollection<IFilter> filters
    ) => new FilterGroup(filterLogic, filters);

    public IFilter WithGroupOfValue(
        EFilterLogic filterLogic,
        Func<FilterBuilderInstance<TContract>, IReadOnlyCollection<IFilter>> filters
    ) => new FilterGroup(filterLogic, filters(this));
}
