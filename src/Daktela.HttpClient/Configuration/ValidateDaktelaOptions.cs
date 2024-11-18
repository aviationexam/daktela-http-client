using Microsoft.Extensions.Options;

namespace Daktela.HttpClient.Configuration;

[OptionsValidator]
public partial class ValidateDaktelaOptions : IValidateOptions<DaktelaOptions>;
