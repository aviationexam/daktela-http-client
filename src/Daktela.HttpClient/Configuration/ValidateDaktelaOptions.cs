using Microsoft.Extensions.Options;

namespace Daktela.HttpClient.Configuration;

#if NET8_0_OR_GREATER
[OptionsValidator]
public partial class ValidateDaktelaOptions : IValidateOptions<DaktelaOptions>;
#else
public class ValidateDaktelaOptions(string? name) : DataAnnotationValidateOptions<DaktelaOptions>(name);
#endif
