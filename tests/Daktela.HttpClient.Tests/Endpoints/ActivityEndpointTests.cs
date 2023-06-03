using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Implementations.Endpoints;
using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.Endpoints;

public partial class ActivityEndpointTests
{
    private readonly TimeSpan _dateTimeOffset = TimeSpan.FromMinutes(90);

    private readonly Mock<IDaktelaHttpClient> _daktelaHttpClientMock = new(MockBehavior.Strict);
    private readonly Mock<IOptions<DaktelaOptions>> _daktelaOptionsMock = new(MockBehavior.Strict);

    private readonly IActivityEndpoint _activityEndpoint;

    public ActivityEndpointTests()
    {
        _daktelaOptionsMock.Setup(x => x.Value)
            .Returns(new DaktelaOptions
            {
                DateTimeOffset = _dateTimeOffset,
            });

        var dateTimeOffsetConverter = new DateTimeOffsetConverter(_daktelaOptionsMock.Object);

        var httpJsonSerializerOptions = new HttpJsonSerializerOptions(dateTimeOffsetConverter);
        _activityEndpoint = new ActivityEndpoint(
            _daktelaHttpClientMock.Object,
            new HttpRequestSerializer(httpJsonSerializerOptions),
            new HttpResponseParser(httpJsonSerializerOptions),
            new PagedResponseProcessor<IActivityEndpoint>()
        );
    }

    [Fact]
    public async Task GetActivityFieldsWorks()
    {
        using var _ = _daktelaHttpClientMock.MockHttpGetListResponse<ActivityField>(
            $"{IActivityEndpoint.UriPrefix}{IActivityEndpoint.UriPostfix}", "fielded-activity-response"
        );
        var responseMetadata = new TotalRecordsResponseBehaviour();

        var cancellationToken = CancellationToken.None;

        var count = 0;
        await foreach (
            var activityFields in _activityEndpoint.GetActivitiesFieldsAsync(
                RequestBuilder.CreateFields(
                    FieldBuilder<ReadActivity>.Create<dynamic>(
                        x => x.Name,
                        x => x.Time!
                    )
                ),
                RequestOptionBuilder.CreateAutoPagingRequestOption(false),
                responseMetadata,
                DaktelaActivityFieldJsonSerializerContext.Default.ListResponseActivityField,
                cancellationToken
            )
        )
        {
            count++;
            Assert.NotNull(activityFields);
            Assert.NotNull(activityFields.Title);
            Assert.NotNull(activityFields.Time);
        }

        Assert.Equal(9, count);
        Assert.Equal(1, responseMetadata.TotalRecords.Count);
        Assert.All(responseMetadata.TotalRecords, x => Assert.Equal(9, x));
    }

    private class ActivityField : IFieldResult
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("time")]
        public DateTimeOffset? Time
        {
            get;
            [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
            set;
        }
    }

    private class TotalRecordsResponseBehaviour : ITotalRecordsResponseBehaviour
    {
        public ICollection<int> TotalRecords { get; } = new List<int>();

        public void SetTotalRecords(int totalRecords) => TotalRecords.Add(totalRecords);
    }

    [JsonSourceGenerationOptions(
        WriteIndented = true,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        GenerationMode = JsonSourceGenerationMode.Serialization
    )]
    [JsonSerializable(typeof(ListResponse<ActivityField>))]
    private partial class DaktelaActivityFieldJsonSerializerContext : JsonSerializerContext
    {
    }
}
