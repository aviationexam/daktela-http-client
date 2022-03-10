using System.Text.Json;

namespace Daktela.HttpClient.Interfaces;

public interface IHttpJsonSerializerOptions
{
    JsonSerializerOptions Value { get; }
}
