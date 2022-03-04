using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Interfaces;

[SuppressMessage("ReSharper", "UnusedTypeParameter", Justification = "Type parameter is used so a library user can replace processor of each endpoint via dependency injection.")]
public interface IPagedResponseProcessor<TEndpoint>
    where TEndpoint : class
{
    IAsyncEnumerable<TContract> InvokeAsync<TContract, TCtx>(
        IRequest request,
        IRequestOption requestOption,
        IResponseBehaviour responseBehaviour,
        TCtx ctx,
        Func<
            IRequest,
            IRequestOption,
            IResponseBehaviour,
            TCtx,
            CancellationToken,
            Task<ListResponse<TContract>>
        > pageRequestCallback,
        CancellationToken cancellationToken
    )
        where TContract : class
        where TCtx : class;
}
