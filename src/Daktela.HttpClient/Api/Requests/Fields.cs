using System.Collections.Generic;

namespace Daktela.HttpClient.Api.Requests;

public record Fields(
    IReadOnlyCollection<string> Items
) : IFields;
