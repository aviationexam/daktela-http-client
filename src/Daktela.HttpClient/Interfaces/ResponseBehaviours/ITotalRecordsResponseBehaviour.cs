namespace Daktela.HttpClient.Interfaces.ResponseBehaviours;

public interface ITotalRecordsResponseBehaviour : IResponseBehaviour
{
    /// <summary>
    /// It may be called multiple times
    /// </summary>
    void SetTotalRecords(int totalRecords);
}
