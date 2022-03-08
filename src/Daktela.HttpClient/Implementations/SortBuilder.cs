using Daktela.HttpClient.Api.Requests;
using System;
using System.Linq.Expressions;

namespace Daktela.HttpClient.Implementations;

public static class SortBuilder<TContract>
    where TContract : class
{
    public static ISorting Ascending<T>(Expression<Func<TContract, T>> propertySelector) => With(propertySelector, ESortDirection.Asc);

    public static ISorting Descending<T>(Expression<Func<TContract, T>> propertySelector) => With(propertySelector, ESortDirection.Desc);

    public static ISorting With<T>(
        Expression<Func<TContract, T>> propertySelector, ESortDirection dir
    ) => new Sorting(PathBuilder<TContract>.Build(propertySelector), dir);
}
