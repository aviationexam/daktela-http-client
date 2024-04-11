using System.Collections.Generic;
using System.Text.Json;

namespace Daktela.HttpClient.Api.Users;

public class ProfileCustomViews : Dictionary<string, JsonElement>
{
    public ProfileCustomViews(IReadOnlyDictionary<string, JsonElement> dictionary) : base(dictionary)
    {
    }

    public ProfileCustomViews()
    {
    }
}
