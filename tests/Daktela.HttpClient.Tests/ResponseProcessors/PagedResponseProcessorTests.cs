using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.ResponseProcessors;

public class PagedResponseProcessorTests
{
    private readonly IPagedResponseProcessor<object> _pagedResponseProcessor;

    public PagedResponseProcessorTests()
    {
        _pagedResponseProcessor = new PagedResponseProcessor<object>();
    }

    [Theory]
    [InlineData(2)]
    public async Task InvokeWorks_DisabledAutoPaging(int take)
    {
        var count = 0;
        await foreach (
            var item in _pagedResponseProcessor.InvokeAsync(
                RequestBuilder.CreateEmpty(),
                RequestOptionBuilder.CreateAutoPagingRequestOption(false),
                ResponseBehaviourBuilder.CreateEmpty(),
                new { },
                (
                    _,
                    _,
                    _,
                    _,
                    _
                ) => Task.FromResult(new ListResponse<object>
                {
                    Error = Array.Empty<string>(),
                    Result = new ListResponse<object>.ResultObject
                    {
                        Data = Enumerable.Range(0, take).Select(_ => new object()).ToList(),
                        Total = take,
                    },
                    Time = new DateTimeOffset()
                }),
                CancellationToken.None
            )
        )
        {
            count++;
            Assert.NotNull(item);
        }

        Assert.NotEqual(0, count);
        Assert.Equal(take, count);
    }

    [Theory]
    [InlineData(0, 2, 2, EResponseBehaviour.OnlyTotalRecords)]
    [InlineData(0, 3, 2, EResponseBehaviour.OnlyTotalRecords)]
    [InlineData(1, 2, 2, EResponseBehaviour.OnlyTotalRecords)]
    [InlineData(1, 2, 4, EResponseBehaviour.OnlyTotalRecords)]
    [InlineData(0, 2, 10, EResponseBehaviour.OnlyTotalRecords)]
    [InlineData(0, 2, 2, EResponseBehaviour.ProcessRequestHooks)]
    [InlineData(0, 3, 2, EResponseBehaviour.ProcessRequestHooks)]
    [InlineData(1, 2, 2, EResponseBehaviour.ProcessRequestHooks)]
    [InlineData(1, 2, 4, EResponseBehaviour.ProcessRequestHooks)]
    [InlineData(0, 2, 10, EResponseBehaviour.ProcessRequestHooks)]
    public async Task InvokeWorks_AutoPaging(int skip, int take, int total, EResponseBehaviour responseBehaviourType)
    {
        var count = 0;
        var responseBehaviour = responseBehaviourType switch
        {
            EResponseBehaviour.OnlyTotalRecords => new TotalRecordsResponseBehaviour(),
            EResponseBehaviour.ProcessRequestHooks => new ProcessRequestHooksResponseBehaviour(),
            _ => throw new ArgumentOutOfRangeException(nameof(responseBehaviourType), responseBehaviourType, null),
        };

        await foreach (
            var item in _pagedResponseProcessor.InvokeAsync(
                RequestBuilder.CreatePaged(new Paging(skip, take)),
                RequestOptionBuilder.CreateAutoPagingRequestOption(true),
                responseBehaviour,
                new
                {
                    total,
                },
                static (
                    request,
                    _,
                    _,
                    ctx,
                    _
                ) =>
                {
                    var paging = Assert.IsAssignableFrom<IPagedQuery>(request).Paging;

                    return Task.FromResult(new ListResponse<object>
                    {
                        Error = Array.Empty<string>(),
                        Result = new ListResponse<object>.ResultObject
                        {
                            Data = Enumerable.Range(
                                0,
                                Math.Min(ctx.total - paging.Skip, paging.Take)
                            ).Select(_ => new object()).ToList(),
                            Total = ctx.total
                        },
                        Time = new DateTimeOffset()
                    });
                },
                CancellationToken.None
            )
        )
        {
            count++;
            Assert.NotNull(item);
        }

        var pages = (int) Math.Ceiling((double) (total - skip) / take);
        Assert.Equal(total - skip, count);
        Assert.Equal(pages, responseBehaviour.TotalRecords.Count);

        if (responseBehaviour is ProcessRequestHooksResponseBehaviour processRequestHooksResponseBehaviour)
        {
            Assert.Equal(pages, processRequestHooksResponseBehaviour.BeforePages.Count);
            Assert.Equal(pages, processRequestHooksResponseBehaviour.AfterPage);

            var pageSkips = new List<int>(pages);
            var expectedPageSkips = Enumerable.Range(0, pages).Select(x => x * take + skip).ToArray();

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            Assert.All(processRequestHooksResponseBehaviour.BeforePages, paging =>
            {
                Assert.NotNull(paging);
                Assert.Equal(take, paging!.Take);

                pageSkips.Add(paging.Skip);
            });
            Assert.Equal(pages, pageSkips.Count);
            var orderedSkips = pageSkips.OrderBy(x => x).ToArray();
            Assert.True(expectedPageSkips.SequenceEqual(orderedSkips), $"Unexpected skips: {string.Join(", ", orderedSkips)}");
        }
        else
        {
            Assert.NotEqual(EResponseBehaviour.ProcessRequestHooks, responseBehaviourType);
        }
    }

    public enum EResponseBehaviour : byte
    {
        OnlyTotalRecords,
        ProcessRequestHooks,
    }

    private class TotalRecordsResponseBehaviour : ITotalRecordsResponseBehaviour
    {
        public ICollection<int> TotalRecords { get; } = new List<int>();

        public void SetTotalRecords(int totalRecords) => TotalRecords.Add(totalRecords);
    }

    private class ProcessRequestHooksResponseBehaviour : TotalRecordsResponseBehaviour, IProcessRequestHooksResponseBehaviour
    {
        public ICollection<Paging?> BeforePages { get; } = new List<Paging?>();
        public int AfterPage { get; private set; }

        public Task BeforePageAsync(Paging? paging, CancellationToken cancellationToken)
        {
            BeforePages.Add(paging);
            return Task.CompletedTask;
        }

        public Task AfterPageAsync(CancellationToken cancellationToken)
        {
            AfterPage++;
            return Task.CompletedTask;
        }
    }
}
