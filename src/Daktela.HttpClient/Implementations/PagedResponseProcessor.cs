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

        if (request is IPagedQuery { Paging: { } paging })
        {
            itemCount = paging.Skip;
        }

        if (requestOption is IAutoPagingRequestOption autoPagingRequestOption)
        {
            autoPaging = autoPagingRequestOption.AutoPaging;
        }

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

            if (responseMetadata is ITotalRecordsResponseMetadata totalRecordsResponseMetadata)
            {
                totalRecordsResponseMetadata.SetTotalRecords(totalItemCount);
            }

            foreach (var item in response.Result.Data)
            {
                yield return item;
            }
        } while (autoPaging && itemCount < totalItemCount);
    }
}
