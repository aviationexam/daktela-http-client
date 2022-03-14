using System.ComponentModel.DataAnnotations;

namespace Daktela.HttpClient.Interfaces;

public interface IContractValidation
{
    ValidationResult? Validate<TContract>(TContract contract, EOperation operation)
        where TContract : class;
}
