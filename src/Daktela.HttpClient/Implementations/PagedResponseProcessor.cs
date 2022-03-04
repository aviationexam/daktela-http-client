using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.Requests.Options;
using Daktela.HttpClient.Interfaces.Responses;
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

        var totalRecordsResponseMetadata = responseMetadata as ITotalRecordsResponseMetadata;
        var processRequestHooksResponseMetadata = responseMetadata as IProcessRequestHooksResponseMetadata;

        do
        {
            var response = await pageRequestCallback(
                request,
                requestOption,
                responseMetadata,
                ctx,
                cancellationToken
            );

            itemCount += response.Result.Data.Count;
            totalItemCount = response.Result.Total;

            totalRecordsResponseMetadata?.SetTotalRecords(totalItemCount);

            if (processRequestHooksResponseMetadata != null)
            {
                await processRequestHooksResponseMetadata.BeforePageAsync(pagedQuery?.Paging, cancellationToken).ConfigureAwait(false);
            }

            foreach (var item in response.Result.Data)
            {
                yield return item;
            }

            if (processRequestHooksResponseMetadata != null)
            {
                await processRequestHooksResponseMetadata.AfterPageAsync(cancellationToken).ConfigureAwait(false);
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
