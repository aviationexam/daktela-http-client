using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations;

internal static class PathBuilder<TContract>
    where TContract : class
{
    public static string Build<T>(Expression<Func<TContract, T>> propertySelector)
    {
        var expr = propertySelector.Body;

        var path = new LinkedList<string>();

        while (expr != null)
        {
            expr = Parse(expr, path);
        }

        return string.Join('.', path.Reverse());
    }

    private static Expression? Parse(
        Expression expression, ICollection<string> path
    ) => expression switch
    {
        ParameterExpression => null, // exit condition
        MemberExpression memberExpression => ParseMemberExpression(memberExpression, path),
        _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, "Unknown type of expression"),
    };

    private static Expression? ParseMemberExpression(MemberExpression expression, ICollection<string> path)
    {
        var member = expression.Member;

        var jsonPropertyNameAttribute = member.GetCustomAttributes<JsonPropertyNameAttribute>(inherit: false)
            .Select(x => x.Name)
            .Distinct()
            .ToList();

        if (jsonPropertyNameAttribute.Count == 0)
        {
            path.Add(member.Name);

            return expression.Expression;
        }

        if (jsonPropertyNameAttribute.Count == 1)
        {
            path.Add(jsonPropertyNameAttribute.Single());

            return expression.Expression;
        }

        throw new Exception($"Unable to parse {nameof(MemberExpression)}");
    }
}