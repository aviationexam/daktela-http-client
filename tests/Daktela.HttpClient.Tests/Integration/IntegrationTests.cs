using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Tests.Infrastructure;
using Daktela.HttpClient.Tests.Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Xunit;
using Xunit.Abstractions;

namespace Daktela.HttpClient.Tests.Integration;

public class IntegrationTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [ManualFact]
    public void EmptyInfrastructureTest()
    {
        using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var daktelaHttpClient = serviceProvider.GetRequiredService<IDaktelaHttpClient>();

        Assert.NotNull(daktelaHttpClient);
    }

    [Theory]
    [ManualInlineData("testing_user")]
    public async Task CreateContact(string name)
    {
        name = $"{name}_{DateTime.Now.Ticks}";
        _testOutputHelper.WriteLine($"Create user {name}");

        await using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var daktelaHttpClient = serviceProvider.GetRequiredService<IDaktelaHttpClient>();
        var httpRequestSerializer = serviceProvider.GetRequiredService<IHttpRequestSerializer>();
        var httpResponseParser = serviceProvider.GetRequiredService<IHttpResponseParser>();

        var cancellationToken = CancellationToken.None;

        await daktelaHttpClient.PostAsync<CreateContact, ReadContact>(
            httpRequestSerializer,
            httpResponseParser,
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            new CreateContact
            {
                Title = $"Title {name}",
                FirstName = null,
                LastName = $"Last {name}",
                Account = "aviationexam",
                User = "administrator",
                Description = null,
                CustomFields = new CustomFields
                {
                    ["number"] = new[] { "123456789" },
                    ["pps_id"] = new[] { "123", "124" },
                    ["freshdesk_id"] = new[] { "12" },
                },
                Name = name,
            },
            cancellationToken
        );

        var encodedName = HttpUtility.UrlEncode(name);

        var updatedContract = await daktelaHttpClient.PutAsync<UpdateContact, ReadContact>(
            httpRequestSerializer,
            httpResponseParser,
            $"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}",
            new UpdateContact
            {
                Title = $"Title {name}",
                LastName = $"Last {name}",
                User = "administrator",
                CustomFields = new CustomFields
                {
                    ["email"] = new[] { "my@email.com" },
                },
            },
            cancellationToken
        );

        Assert.NotNull(updatedContract);
        Assert.NotNull(updatedContract.User);

        await daktelaHttpClient.DeleteAsync($"{IContactEndpoint.UriPrefix}/{encodedName}{IContactEndpoint.UriPostfix}", cancellationToken);
    }

    [ManualFact]
    public async Task GetTicketsCategories()
    {
        await using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var ticketsCategoriesEndpoint = serviceProvider.GetRequiredService<ITicketsCategoryEndpoint>();

        var cancellationToken = CancellationToken.None;

        var request = RequestBuilder.CreateEmpty();
        var ticketCategories = new List<Category>();

        var categories = ticketsCategoriesEndpoint.GetTicketsCategoriesAsync(
            request,
            RequestOptionBuilder.CreateAutoPagingRequestOption(false),
            ResponseBehaviourBuilder.CreateEmpty(),
            cancellationToken
        ).WithCancellation(cancellationToken).ConfigureAwait(false);

        await foreach (var category in categories)
        {
            ticketCategories.Add(category);
        }

        Assert.NotEmpty(ticketCategories);
    }
}
