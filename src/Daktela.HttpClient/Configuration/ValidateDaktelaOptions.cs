using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Daktela.HttpClient.Configuration;

#if NET8_0_OR_GREATER
[OptionsValidator]
public partial class ValidateDaktelaOptions() : IValidateOptions<DaktelaOptions>
{
    public ValidateDaktelaOptions(
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        string name
    ) : this()
    {
    }
}
#else
public class ValidateDaktelaOptions(string? name) : DataAnnotationValidateOptions<DaktelaOptions>(name);
#endif
