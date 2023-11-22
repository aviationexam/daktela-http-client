using Daktela.HttpClient.Attributes;
using Daktela.HttpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Daktela.HttpClient.Implementations;

public class ContractValidation : IContractValidation
{
    public ValidationResult? Validate<
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicFields |
            DynamicallyAccessedMemberTypes.PublicProperties
        )]
    TContract
    >(TContract contract, EOperation operation)
        where TContract : class
    {
        var type = typeof(TContract);

        var errors = new List<string>();

        foreach (var memberInfo in GetMembers(type))
        {
            var daktelaRequirementAttributes = memberInfo.GetCustomAttributes()
                .OfType<DaktelaRequirementAttribute>()
                .Where(x => x.ApplyOnOperation.HasFlag(operation));

            foreach (var daktelaRequirementAttribute in daktelaRequirementAttributes)
            {
                Validate(contract, memberInfo, daktelaRequirementAttribute, errors);
            }
        }

        if (errors.Any())
        {
            return new ValidationResult("Following members are required:", errors);
        }

        return ValidationResult.Success;
    }

    private void Validate<TContract>(
        TContract contract,
        MemberInfo memberInfo,
        DaktelaRequirementAttribute attribute,
        ICollection<string> errors
    ) where TContract : class
    {
        var (value, memberType) = memberInfo switch
        {
            FieldInfo fieldInfo => (fieldInfo.GetValue(contract), fieldInfo.FieldType),
            PropertyInfo propertyInfo => (propertyInfo.GetValue(contract), propertyInfo.PropertyType),
            _ => throw new ArgumentOutOfRangeException(nameof(memberInfo.MemberType), memberInfo.MemberType, null),
        };

        if (memberType.IsEnum && value is not null)
        {
            return;
        }

        switch (attribute)
        {
            case DaktelaNonZeroValueAttribute daktelaNonZeroValueAttribute:
                if (daktelaNonZeroValueAttribute.IsValid(value))
                {
                    return;
                }

                errors.Add(memberInfo.Name);
                return;
        }

        switch (value)
        {
            case null:
                errors.Add(memberInfo.Name);
                break;
            case string stringValue:
                if (string.IsNullOrEmpty(stringValue))
                {
                    errors.Add(memberInfo.Name);
                }

                break;
            default:
                throw new NotSupportedException($"The {nameof(value)} type {value.GetType()} is not supported");
        }
    }

    private IEnumerable<MemberInfo> GetMembers(
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicFields |
            DynamicallyAccessedMemberTypes.PublicProperties
        )]
        Type type)
    {
        foreach (var fieldInfo in type.GetFields())
        {
            yield return fieldInfo;
        }

        foreach (var propertyInfo in type.GetProperties())
        {
            yield return propertyInfo;
        }
    }
}
