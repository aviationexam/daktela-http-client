using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Daktela.HttpClient.Interfaces;

public interface IContractValidation
{
    ValidationResult? Validate<
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicFields |
            DynamicallyAccessedMemberTypes.PublicProperties
        )]
        TContract
    >(
        TContract contract, EOperation operation
    ) where TContract : class;
}
