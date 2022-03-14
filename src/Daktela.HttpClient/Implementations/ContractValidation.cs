using Daktela.HttpClient.Attributes;
using Daktela.HttpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Daktela.HttpClient.Implementations;

public class ContractValidation : IContractValidation
{
    public ValidationResult? Validate<TContract>(TContract contract, EOperation operation)
        where TContract : class
    {
        var type = typeof(TContract);

        var errors = new List<string>();

        foreach (var memberInfo in GetMembers(type))
        {
            var daktelaRequirementAttribute = memberInfo.GetCustomAttribute<DaktelaRequirementAttribute>();

            if (daktelaRequirementAttribute != null && daktelaRequirementAttribute.ApplyOnOperation.HasFlag(operation))
            {
                var value = memberInfo switch
                {
                    FieldInfo fieldInfo => fieldInfo.GetValue(contract),
                    PropertyInfo propertyInfo => propertyInfo.GetValue(contract),
                    _ => throw new ArgumentOutOfRangeException(nameof(memberInfo.MemberType), memberInfo.MemberType, null),
                };

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
        }

        if (errors.Any())
        {
            return new ValidationResult("Following members are required:", errors);
        }

        return ValidationResult.Success;
    }

    private IEnumerable<MemberInfo> GetMembers(Type type)
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
