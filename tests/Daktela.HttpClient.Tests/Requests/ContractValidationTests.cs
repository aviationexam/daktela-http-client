using Daktela.HttpClient.Attributes;
using Daktela.HttpClient.Implementations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Daktela.HttpClient.Tests.Requests;

public class ContractValidationTests
{
    private readonly ContractValidation _contractValidation = new();

    [Theory]
    [InlineData(EOperation.Read)]
    [InlineData(EOperation.Create)]
    [InlineData(EOperation.Update)]
    [InlineData(EOperation.Delete)]
    public void FullContractValidationWorks(EOperation operation)
    {
        var contract = new Contract
        {
            A = "a",
            B = "b",
            C = "c",
        };

        var result = _contractValidation.Validate(contract, operation);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Theory]
    [InlineData(EOperation.Read, new string[] { })]
    [InlineData(EOperation.Create, new[] { nameof(Contract.A), nameof(Contract.B) })]
    [InlineData(EOperation.Update, new[] { nameof(Contract.A) })]
    [InlineData(EOperation.Delete, new string[] { })]
    public void EmptyValidationWorks(EOperation operation, string[] errorMembers)
    {
        var contract = new Contract
        {
            A = null!,
            B = null!,
            C = null!,
        };

        var result = _contractValidation.Validate(contract, operation);

        if (errorMembers.Any())
        {
            Assert.NotEqual(ValidationResult.Success, result);
            Assert.Equal("Following members are required:", result!.ErrorMessage);
            Assert.Equal(errorMembers, result.MemberNames);
        }
        else
        {
            Assert.Equal(ValidationResult.Success, result);
        }
    }

    private class Contract
    {
        [DaktelaRequirement(EOperation.Create | EOperation.Update)]
        public string A { get; set; } = null!;

        [DaktelaRequirement(EOperation.Create)]
        public string B { get; set; } = null!;

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        public string C { get; set; } = null!;
    }
}
