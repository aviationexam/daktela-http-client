using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        var enumsConverter = new EnumsConverter<TEnum>();

        return new Filter(
            PathBuilder<TContract>.Build(propertySelector), filterOperator,
            enumsConverter.ReverseMapping[enumValue],
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
