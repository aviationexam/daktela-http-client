namespace Daktela.HttpClient.Interfaces.Responses;

public interface ITotalRecordsResponseMetadata : IResponseMetadata
{
    /// <summary>
    /// It may be called multiple times
    /// </summary>
    void SetTotalRecords(int totalRecords);
}
