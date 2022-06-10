using Daktela.HttpClient.Interfaces.Requests;
using System.Diagnostics.CodeAnalysis;

namespace Daktela.HttpClient.Exceptions;

public class DaktelaFieldsException<TContract, TReturn> : DaktelaException
    where TContract : class
    where TReturn : class, IFieldResult
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public string PropertyName { get; }

    public string? SourceJsonName { get; }
    public string? TargetJsonName { get; }

    public DaktelaFieldsException(
        string propertyName
    ) : base($"The property {propertyName} from the {typeof(TReturn).Name} does not exist in the {typeof(TContract).Name}")
    {
        PropertyName = propertyName;
    }

    public DaktelaFieldsException(
        string propertyName,
        string? sourceJsonName,
        string? targetJsonName
    ) : base($"The property {propertyName} from the {typeof(TReturn).Name} has different JSON name in the {typeof(TContract).Name}. Source:{sourceJsonName} <> Target:{targetJsonName}")
    {
        PropertyName = propertyName;
        SourceJsonName = sourceJsonName;
        TargetJsonName = targetJsonName;
    }
}
