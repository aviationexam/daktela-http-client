using Daktela.HttpClient.Interfaces.Requests.Options;

namespace Daktela.HttpClient.Implementations.Requests.Options;

internal record AutoPagingRequestOption(bool AutoPaging) : IAutoPagingRequestOption;
