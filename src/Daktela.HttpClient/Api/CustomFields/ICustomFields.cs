using System.Collections.Generic;

namespace Daktela.HttpClient.Api.CustomFields;

public interface ICustomFields : IReadOnlyDictionary<string, IReadOnlyCollection<string>>;
