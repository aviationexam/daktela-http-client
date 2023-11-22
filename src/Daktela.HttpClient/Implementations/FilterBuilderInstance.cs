using Aviationexam.GeneratedJsonConverters;
using Aviationexam.GeneratedJsonConverters.Attributes;
using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Requests;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
        if (DaktelaJsonSerializerContext.Default.Options.GetConverter(typeof(TEnum)) is not EnumJsonConvertor<TEnum> converter)
        {
            throw new Exception($"Enable to serialize {typeof(TEnum)}. Decorate it with {nameof(EnumJsonConverterAttribute)} to support serialization.");
        }

        var serializedEnum = Encoding.UTF8.GetString(
            converter.ToFirstEnumName(enumValue)
        );

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
