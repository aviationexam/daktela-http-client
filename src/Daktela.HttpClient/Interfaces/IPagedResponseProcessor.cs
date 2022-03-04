using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces;

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface IPagedResponseProcessor<TEndpoint>
    where TEndpoint : class
{
    IAsyncEnumerable<TContract> InvokeAsync<TContract, TCtx>(
        IRequest request,
        IRequestOption requestOption,
        IResponseMetadata responseMetadata,
        TCtx ctx,
        Func<
            IRequest,
            IRequestOption,
            IResponseMetadata,
            TCtx,
            CancellationToken,
            Task<ListResponse<TContract>>
        > pageRequestCallback,
        CancellationToken cancellationToken
    )
        where TContract : class
        where TCtx : class;
}
