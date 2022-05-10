using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.CustomFields;
using Daktela.HttpClient.Api.Files;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Tests.Infrastructure;
using Daktela.HttpClient.Tests.Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

    [Theory]
    [ManualInlineData("testing_user_637872717018821366", null!)]
    public async Task CreateTicket(string contact, string? user)
    {
        await using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var ticketEndpoint = serviceProvider.GetRequiredService<ITicketEndpoint>();
        var activityEndpoint = serviceProvider.GetRequiredService<IActivityEndpoint>();

        var cancellationToken = CancellationToken.None;

        var createTicket = new CreateTicket
        {
            Category = "categories_62726c1c9efa6180321068",
            Title = "Api test",
            Contact = contact,
            User = user,
            Stage = EStage.Open,
            Statuses = new List<string> { "statuses_62726da982f92110000280" },
            CustomFields = new CustomFields
            {
                ["pps_conversation_id"] = new[] { "123" },
            }
        };

        var ticket = await ticketEndpoint.CreateTicketAsync(createTicket, cancellationToken);
        Assert.NotNull(ticket);

        var createActivity = new CreateActivity
        {
            Ticket = ticket.Name,
            Title = "Test of SPF & DKIM",
            Name = $"activities-{ticket.Name}-{DateTime.Now.Ticks}",
            Type = EActivityType.Comment,
            Description = "Text komentáře",
            Action = EAction.Open,
            User = user
        };

        var activity = await activityEndpoint.CreateActivityAsync(createActivity, cancellationToken);
        Assert.NotNull(activity);

        await ticketEndpoint.DeleteTicketAsync(ticket.Name, cancellationToken);
    }

    [Theory]
    [ManualInlineData(9673, null!, "https://www.daktela.com/wp-content/uploads/2020/04/cropped-512x512-32x32.png", "favicon.png")]
    public async Task CreateTicketActivity(int ticketId, string? user, string fileUrl, string fileName)
    {
        await using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var daktelaHttpClient = serviceProvider.GetRequiredService<IDaktelaHttpClient>();
        var fileEndpoint = serviceProvider.GetRequiredService<IFileEndpoint>();
        var activityEndpoint = serviceProvider.GetRequiredService<IActivityEndpoint>();

        var cancellationToken = CancellationToken.None;

        #region UploadFile

        using var sourceHttpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(fileUrl, UriKind.Absolute));

        using var sourceHttpResponseMessage = await daktelaHttpClient.RawSendAsync(sourceHttpRequest, cancellationToken);

        var sourceStream = await sourceHttpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

        var streamLength = sourceStream.Length;
        var fileIdentifier = await fileEndpoint.UploadFileAsync(
            sourceStream,
            fileName,
            cancellationToken
        );

        #endregion

        var createActivity = new CreateActivity
        {
            Ticket = ticketId,
            Title = "Activity test",
            // Name = $"activities-{ticketId}-{DateTime.Now.Ticks}",
            Type = EActivityType.Comment,
            Description = "Text komentáře",
            Action = EAction.Open,
            User = user,
            AddFiles = new[]
            {
                // this part does not work for comments...
                new File
                {
                    FileIdentifier = fileIdentifier,
                    FileName = fileName,
                    Size = streamLength,
                    Type = null,
                }
            }
        };

        var activity = await activityEndpoint.CreateActivityAsync(createActivity, cancellationToken);
        Assert.NotNull(activity);
    }

    [Theory]
    [ManualInlineData(9673, "https://www.daktela.com/wp-content/uploads/2020/04/cropped-512x512-32x32.png", "favicon.png")]
    public async Task CreateTicketActivityThroughTicket(int ticketId, string fileUrl, string fileName)
    {
        await using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var daktelaHttpClient = serviceProvider.GetRequiredService<IDaktelaHttpClient>();
        var fileEndpoint = serviceProvider.GetRequiredService<IFileEndpoint>();
        var ticketEndpoint = serviceProvider.GetRequiredService<ITicketEndpoint>();

        var cancellationToken = CancellationToken.None;

        #region UploadFile

        using var sourceHttpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(fileUrl, UriKind.Absolute));

        using var sourceHttpResponseMessage = await daktelaHttpClient.RawSendAsync(sourceHttpRequest, cancellationToken);

        var sourceStream = await sourceHttpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

        var streamLength = sourceStream.Length;
        var fileIdentifier = await fileEndpoint.UploadFileAsync(
            sourceStream,
            fileName,
            cancellationToken
        );

        #endregion

        var originalTicket = await ticketEndpoint.GetTicketAsync(ticketId, cancellationToken);
        Assert.NotNull(originalTicket);

        var updateTicket = originalTicket.ToUpdateTicket();
        updateTicket.Comment = $"Text komentáře {DateTime.Now.Date}";
        updateTicket.AddFiles = new[]
        {
            new File
            {
                FileIdentifier = fileIdentifier,
                FileName = fileName,
                Size = streamLength,
                Type = null,
            }
        };

        var updatedTicket = await ticketEndpoint.UpdateTicketAsync(ticketId, updateTicket, cancellationToken);
        Assert.NotNull(updatedTicket);
    }

    [Theory]
    [ManualInlineData("https://www.daktela.com/wp-content/uploads/2020/04/cropped-512x512-32x32.png", ".png")]
    public async Task UploadFileWorks(string fileUrl, string fileExtension)
    {
        await using var serviceProvider = TestHttpClientFactory.CreateServiceProvider();

        var daktelaHttpClient = serviceProvider.GetRequiredService<IDaktelaHttpClient>();
        var fileEndpoint = serviceProvider.GetRequiredService<IFileEndpoint>();

        var cancellationToken = CancellationToken.None;

        using var sourceHttpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(fileUrl, UriKind.Absolute));

        using var sourceHttpResponseMessage = await daktelaHttpClient.RawSendAsync(sourceHttpRequest, cancellationToken);

        var sourceStream = await sourceHttpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

        var fileName = $"favicon{fileExtension}";
        var fileIdentifier = await fileEndpoint.UploadFileAsync(
            sourceStream,
            fileName,
            cancellationToken
        );

        Assert.NotNull(fileIdentifier);
        Assert.EndsWith(fileExtension, fileIdentifier);

        var removeFileSuccess = await fileEndpoint.RemoveFileAsync(
            fileName,
            cancellationToken
        );

        Assert.True(removeFileSuccess);
    }
}
