using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Api.Responses.Errors;
using Daktela.HttpClient.Api.Users;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Exceptions;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Implementations.Endpoints;
using Daktela.HttpClient.Implementations.JsonConverters;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Queries;
using Daktela.HttpClient.Interfaces.ResponseBehaviours;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.Endpoints;

public class ContactEndpointTests
{
    private readonly TimeSpan _dateTimeOffset = TimeSpan.FromMinutes(90);

    private readonly Mock<IDaktelaHttpClient> _daktelaHttpClientMock = new(MockBehavior.Strict);
    private readonly Mock<IOptions<DaktelaOptions>> _daktelaOptionsMock = new(MockBehavior.Strict);

    private readonly IContactEndpoint _contactEndpoint;

    public ContactEndpointTests()
    {
        _daktelaOptionsMock.Setup(x => x.Value)
            .Returns(new DaktelaOptions
            {
                DateTimeOffset = _dateTimeOffset,
            });

        var dateTimeOffsetConverter = new DateTimeOffsetConverter(_daktelaOptionsMock.Object);

        var httpJsonSerializerOptions = new HttpJsonSerializerOptions(dateTimeOffsetConverter);
        _contactEndpoint = new ContactEndpoint(
            _daktelaHttpClientMock.Object,
            new HttpRequestSerializer(httpJsonSerializerOptions),
            new HttpResponseParser(httpJsonSerializerOptions),
            new PagedResponseProcessor<IContactEndpoint>()
        );
    }

    [Fact]
    public async Task GetSimpleContactWorks()
    {
        const string name = "testing_user";

        using var _ = _daktelaHttpClientMock.MockHttpGetResponse<ReadContact>(
            $"{IContactEndpoint.UriPrefix}/{name}{IContactEndpoint.UriPostfix}", "simple-contact"
        );

        var contact = await _contactEndpoint.GetContactAsync(name);

        Assert.NotNull(contact);
        Assert.Equal(name, contact.Name);
        Assert.Equal(new DateTimeOffset(2022, 3, 2, 14, 6, 5, _dateTimeOffset), contact.Edited);
        Assert.Equal(new DateTimeOffset(2022, 3, 2, 14, 6, 5, _dateTimeOffset), contact.Created);
        Assert.Null(contact.User);
        Assert.Null(contact.Account);
    }

    [Fact]
    public async Task GetSimpleContactWithUserWorks()
    {
        const string name = "test_user_with_user";

        using var _ = _daktelaHttpClientMock.MockHttpGetResponse<ReadContact>(
            $"{IContactEndpoint.UriPrefix}/{name}{IContactEndpoint.UriPostfix}", "simple-contact-with-user"
        );

        var contact = await _contactEndpoint.GetContactAsync(name);

        Assert.NotNull(contact);
        Assert.Equal(name, contact.Name);
        Assert.Equal(new DateTimeOffset(2022, 3, 2, 15, 51, 17, _dateTimeOffset), contact.Created);
        Assert.Equal(new DateTimeOffset(2022, 3, 2, 16, 1, 27, _dateTimeOffset), contact.Edited);
        Assert.NotNull(contact.User);
        Assert.Null(contact.Account);

        Assert.Equal("administrator", contact.User!.Name);
        Assert.Equal("admin", contact.User.Role.Name);
        Assert.Equal("admin", contact.User.Profile.Name);
    }

    [Fact]
    public async Task GetContactsWorks()
    {
        using var _ = _daktelaHttpClientMock.MockHttpGetListResponse<ReadContact>(
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}", "contacts"
        );

        var responseMetadata = new TotalRecordsResponseBehaviour();

        var cancellationToken = CancellationToken.None;

        var count = 0;
        var users = new List<User>();
        await foreach (
            var contact in _contactEndpoint.GetContactsAsync(
                RequestBuilder.CreatePaged(new Paging(0, 2)),
                RequestOptionBuilder.CreateAutoPagingRequestOption(false),
                responseMetadata,
                cancellationToken
            ).WithCancellation(cancellationToken)
        )
        {
            count++;
            Assert.NotNull(contact);
            Assert.Null(contact.Account);
            Assert.NotNull(contact.CustomFields);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            Assert.All(contact.CustomFields!, x =>
            {
                Assert.NotEmpty(x.Key);
                Assert.Empty(x.Value);
            });

            if (contact.User != null)
            {
                users.Add(contact.User);
            }
        }

        Assert.Equal(2, count);
        var totalRecords = Assert.Single(responseMetadata.TotalRecords);
        Assert.Equal(4, totalRecords);

        var user = Assert.Single(users);
        Assert.Equal("administrator", user.Name);
        Assert.Equal("admin", user.Role.Name);
        Assert.Equal("admin", user.Profile.Name);
        Assert.Equal(ECanTransferCall.BlindAndAssistedTransfer, user.Profile.CanTransferCall);
        Assert.Equal(EExtensionState.Offline, user.ExtensionState);
        Assert.Equal(ERecordAtCallStart.Disabled, user.RecordAtCallStart);
        Assert.Null(user.Acl);
        Assert.False(user.Deleted);
        Assert.False(user.Deactivated);
    }

    [Fact]
    public async Task GetContactsWorks_AutoPaginate()
    {
        using var _ = _daktelaHttpClientMock.MockHttpGetListResponse<ReadContact>(
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            request => ((IPagedQuery) request).Paging == new Paging(0, 2)
                       && (Sorting) ((ISortableQuery) request).Sorting.Single() == new Sorting("edited", ESortDirection.Asc),
            "contacts"
        );
        using var secondResponse = _daktelaHttpClientMock.MockHttpGetListResponse<ReadContact>(
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            request => ((IPagedQuery) request).Paging == new Paging(2, 2)
                       && (Sorting) ((ISortableQuery) request).Sorting.Single() == new Sorting("edited", ESortDirection.Asc),
            "contacts"
        );

        var cancellationToken = CancellationToken.None;
        var responseMetadata = new TotalRecordsResponseBehaviour();

        var count = 0;
        await foreach (
            var contact in _contactEndpoint.GetContactsAsync(
                RequestBuilder.CreatePaged(new Paging(0, 2))
                    .WithSortable(SortBuilder<ReadContact>.Ascending(x => x.Edited)),
                RequestOptionBuilder.CreateAutoPagingRequestOption(true),
                responseMetadata,
                cancellationToken
            ).WithCancellation(cancellationToken)
        )
        {
            count++;
            Assert.NotNull(contact);
            Assert.Null(contact.Account);
        }

        Assert.Equal(4, count);
        Assert.Equal(2, responseMetadata.TotalRecords.Count);
        Assert.All(responseMetadata.TotalRecords, x => Assert.Equal(4, x));
    }

    [Fact]
    public async Task CreateContactsFailed_BadRequest()
    {
        const string name = "testing_user";

        using var _ = _daktelaHttpClientMock.MockHttpPostResponse_BadRequest<CreateContact, ReadContact>(
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}",
            _ => true,
            "create-contract-bad-request"
        );

        var contract = new CreateContact
        {
            Title = $"Title {name}",
            FirstName = "Title",
            LastName = "testing_user",
            Account = null,
            User = "administrator",
            Description = null,
            CustomFields = null,
            Name = name
        };

        var exception = await Assert.ThrowsAsync<BadRequestException<ReadContact>>(() => _contactEndpoint.CreateContactAsync(contract));

        Assert.NotNull(exception.Contract);
        Assert.NotNull(exception.ErrorsResponse);

        var result = exception.Contract;
        Assert.Equal(contract.Title, result.Title);
        Assert.Equal(contract.FirstName, result.FirstName);
        Assert.Equal(contract.LastName, result.LastName);
        Assert.Null(result.Account);
        Assert.Null(result.User);
        Assert.Equal(contract.Description, result.Description);
        Assert.NotNull(result.CustomFields);
        Assert.Equal(contract.Name, result.Name);

        var error = Assert.IsType<ComplexErrorResponse>(exception.ErrorsResponse);
        Assert.NotNull(error.Form);
        Assert.Null(error.Primary);

        var errorForm = Assert.Single(error.Form);
        Assert.Equal("user", errorForm.Key);
        var errorFormMessage = Assert.IsType<ErrorFormMessage>(errorForm.Value);
        Assert.Equal("Chyba cizího klíče", errorFormMessage.ErrorMessage);
    }

    [Fact]
    public async Task DeleteContactsWorks()
    {
        const string name = "testing_user";

        _daktelaHttpClientMock.MockHttpDeleteResponse($"{IContactEndpoint.UriPrefix}/{name}{IContactEndpoint.UriPostfix}");

        await _contactEndpoint.DeleteContactAsync(name);
    }

    private class TotalRecordsResponseBehaviour : ITotalRecordsResponseBehaviour
    {
        public ICollection<int> TotalRecords { get; } = new List<int>();

        public void SetTotalRecords(int totalRecords) => TotalRecords.Add(totalRecords);
    }
}
