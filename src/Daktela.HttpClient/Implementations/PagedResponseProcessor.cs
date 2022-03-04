using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Daktela.HttpClient.Implementations;

public class PagedResponseProcessor<TEndpoint> : IPagedResponseProcessor<TEndpoint>
    where TEndpoint : class
{
    public async IAsyncEnumerable<TContract> InvokeAsync<TContract, TCtx>(
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
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
        where TContract : class
        where TCtx : class
    {
        // ReSharper disable once RedundantAssignment
        var itemCount = 0;
        int totalItemCount;
        // ReSharper disable once RedundantAssignment
        var autoPaging = false;

        var pagedQuery = request as IPagedQuery;
        if (pagedQuery != null)
        {
            itemCount = pagedQuery.Paging.Skip;
        }

        if (requestOption is IAutoPagingRequestOption autoPagingRequestOption)
        {
            autoPaging = autoPagingRequestOption.AutoPaging;
        }

        var totalRecordsResponseBehaviour = responseBehaviour as ITotalRecordsResponseBehaviour;
        var processRequestHooksResponseBehaviour = responseBehaviour as IProcessRequestHooksResponseBehaviour;

        do
        {
            var response = await pageRequestCallback(
                request,
                requestOption,
                responseBehaviour,
                ctx,
                cancellationToken
            );

            itemCount += response.Result.Data.Count;
            totalItemCount = response.Result.Total;

            totalRecordsResponseBehaviour?.SetTotalRecords(totalItemCount);

            if (processRequestHooksResponseBehaviour != null)
            {
                await processRequestHooksResponseBehaviour.BeforePageAsync(pagedQuery?.Paging, cancellationToken).ConfigureAwait(false);
            }

            foreach (var item in response.Result.Data)
            {
                yield return item;
            }

            if (processRequestHooksResponseBehaviour != null)
            {
                await processRequestHooksResponseBehaviour.AfterPageAsync(cancellationToken).ConfigureAwait(false);
            }

            if (autoPaging && pagedQuery != null)
            {
                pagedQuery.Paging = pagedQuery.Paging with
                {
                    Skip = pagedQuery.Paging.Skip + pagedQuery.Paging.Take
                };
            }
        } while (autoPaging && itemCount < totalItemCount);
    }
}
