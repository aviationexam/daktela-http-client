namespace Daktela.HttpClient.Api.Requests;

/// <summary>
/// A marking interface, a library user is not supposed to implement it by themself
/// </summary>
public interface ISorting
{
    string Field { get; }

    ESortDirection Dir { get; }
}
