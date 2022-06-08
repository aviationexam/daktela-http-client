using Daktela.HttpClient.Api.Requests;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Daktela.HttpClient.Implementations;

public static class FilterBuilder<TContract>
    where TContract : class
{
    public static IFilter WithValue<T>(
        Expression<Func<TContract, T>> propertySelector, EFilterOperator filterOperator, string value, string? type = null
    ) => new FilterBuilderInstance<TContract>().WithValue(propertySelector, filterOperator, value, type);

    public static IFilter WithEnumValue<T, TEnum>(
        Expression<Func<TContract, T>> propertySelector, EFilterOperator filterOperator, TEnum enumValue, string? type = null
    ) where TEnum : struct, Enum => new FilterBuilderInstance<TContract>()
        .WithEnumValue(propertySelector, filterOperator, enumValue, type);

    public static IFilter WithGroupOfValue(
        EFilterLogic filterLogic,
        IReadOnlyCollection<IFilter> filters
    ) => new FilterBuilderInstance<TContract>().WithGroupOfValue(filterLogic, filters);

    public static IFilter WithGroupOfValue(
        EFilterLogic filterLogic,
        Func<FilterBuilderInstance<TContract>, IReadOnlyCollection<IFilter>> filters
    ) => new FilterBuilderInstance<TContract>().WithGroupOfValue(filterLogic, filters);
}
