namespace Daktela.HttpClient.Interfaces.Requests.Options;

public interface IAutoPagingRequestOption : IRequestOption
{
    bool AutoPaging { get; }
}
