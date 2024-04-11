using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Responses.Errors;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Implementations.Endpoints;
using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Requests;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.Endpoints;

public partial class ActivityEndpointTests
{
    private readonly TimeSpan _dateTimeOffset = TimeSpan.FromMinutes(90);

    private readonly Mock<IDaktelaHttpClient> _daktelaHttpClientMock = new(MockBehavior.Strict);

    private readonly IActivityEndpoint _activityEndpoint;

    public ActivityEndpointTests()
    {
        DaktelaJsonSerializerContext.SerializationDateTimeOffset = _dateTimeOffset;
        DaktelaActivityFieldJsonSerializerContext.SerializationDateTimeOffset = _dateTimeOffset;

        _activityEndpoint = new ActivityEndpoint(
            _daktelaHttpClientMock.Object,
            new HttpRequestSerializer(),
            new HttpResponseParser(),
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
                DaktelaActivityFieldJsonSerializerContext.CustomConverters.ListResponseActivityField,
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
        Assert.Single(responseMetadata.TotalRecords);
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
        GenerationMode = JsonSourceGenerationMode.Default
    )]
    [JsonSerializable(typeof(ComplexErrorResponse))]
    [JsonSerializable(typeof(ErrorFormMessages))]
    [JsonSerializable(typeof(NestedErrorForm))]
    [JsonSerializable(typeof(PlainErrorResponse))]
    [JsonSerializable(typeof(ListResponse<ActivityField>))]
    [JsonSerializable(typeof(IReadOnlyDictionary<string, IReadOnlyCollection<string>>))]
    private partial class DaktelaActivityFieldJsonSerializerContext : JsonSerializerContext
    {
        private static TimeSpan _serializationDateTimeOffset;

        public static TimeSpan SerializationDateTimeOffset
        {
            get => _serializationDateTimeOffset;
            set
            {
                _serializationDateTimeOffset = value;
                _convertersContext = null;
            }
        }

        private static JsonSerializerOptions ConvertersContextOptions
        {
            get
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                    IgnoreReadOnlyFields = false,
                    IgnoreReadOnlyProperties = false,
                    IncludeFields = false,
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters =
                    {
                        new DateTimeOffsetConverter(SerializationDateTimeOffset),
                        new TimeSpanConverter(),
                        new ReadActivityConverter(),
                        new CustomFieldsConverter(),
                        new ProfileCustomViewsConverter(),
                        new EmailActivityOptionsHeadersAddressConverter(),
                        new ErrorResponseConverter(),
                        new ErrorFormConverter(),
                    },
                };

                DaktelaJsonSerializerContext.UseEnumConverters(jsonSerializerOptions.Converters);

                return jsonSerializerOptions;
            }
        }

        private static DaktelaActivityFieldJsonSerializerContext? _convertersContext;

        /// <summary>
        /// The default <see cref="global::System.Text.Json.Serialization.JsonSerializerContext"/> associated with a default <see cref="global::System.Text.Json.JsonSerializerOptions"/> instance.
        /// </summary>
        public static DaktelaActivityFieldJsonSerializerContext CustomConverters => _convertersContext
            ??= new DaktelaActivityFieldJsonSerializerContext(
                new JsonSerializerOptions(ConvertersContextOptions)
            );
    }
}
