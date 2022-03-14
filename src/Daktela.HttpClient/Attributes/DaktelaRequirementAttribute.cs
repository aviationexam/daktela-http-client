using System;

namespace Daktela.HttpClient.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class DaktelaRequirementAttribute : Attribute
{
    public EOperation ApplyOnOperation { get; }

    public DaktelaRequirementAttribute(EOperation applyOnOperation)
    {
        ApplyOnOperation = applyOnOperation;
    }
}
