using System.Collections.Generic;

namespace Daktela.HttpClient.Api.Requests;

public record Fields(
    ICollection<string> Items
) : IFields;
