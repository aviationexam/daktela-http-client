using Daktela.HttpClient.Api.Requests;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Daktela.HttpClient.Implementations;

public static class FieldBuilder<TContract>
    where TContract : class
{
    public static IFields Create<T>(
        params Expression<Func<TContract, T>>[] propertySelectors
    ) => new Fields(propertySelectors.Select(PathBuilder<TContract>.Build).ToArray());
}
