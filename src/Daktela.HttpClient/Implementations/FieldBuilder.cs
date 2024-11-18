using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Daktela.HttpClient.Implementations;

public static class FieldBuilder<
    [DynamicallyAccessedMembers(
        DynamicallyAccessedMemberTypes.PublicProperties
    )]
TContract
>
    where TContract : class
{
    [RequiresDynamicCode("Not fully supported for AOT")]
    public static IFields Create<T>(
        params Expression<Func<TContract, T>>[] propertySelectors
    ) => new Fields(propertySelectors.Select(PathBuilder<TContract>.Build).ToArray());

    public static IFields CreateFor<
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicProperties
        )]
    TReturn
    >() where TReturn : class, IFieldResult
    {
        var sourceType = typeof(TContract);
        var targetType = typeof(TReturn);

        var sourceProperties = sourceType.GetProperties().ToDictionary(x => x.Name);
        var targetProperties = targetType.GetProperties();

        var fields = new List<string>(targetProperties.Length);

        foreach (var property in targetProperties)
        {
            if (sourceProperties.ContainsKey(property.Name))
            {
                var targetJsonName = property.GetCustomAttributes<JsonPropertyNameAttribute>(inherit: false)
                    .Select(x => x.Name)
                    .Distinct()
                    .Single();

                var sourceJsonName = sourceProperties[property.Name].GetCustomAttributes<JsonPropertyNameAttribute>(inherit: false)
                    .Select(x => x.Name)
                    .Distinct()
                    .Single();

                if (!targetJsonName.Equals(sourceJsonName, StringComparison.InvariantCulture))
                {
                    throw new DaktelaFieldsException<TContract, TReturn>(property.Name, sourceJsonName, targetJsonName);
                }

                fields.Add(targetJsonName);
            }
            else
            {
                throw new DaktelaFieldsException<TContract, TReturn>(property.Name);
            }
        }

        return new Fields(fields);
    }
}
