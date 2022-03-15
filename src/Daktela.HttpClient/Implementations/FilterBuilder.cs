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
    ) => new Filter(PathBuilder<TContract>.Build(propertySelector), filterOperator, value, type);

    public static IFilter WithGroupOfValue(
        EFilterLogic filterLogic,
        IReadOnlyCollection<IFilter> filters
    ) => new FilterGroup(filterLogic, filters);
}
