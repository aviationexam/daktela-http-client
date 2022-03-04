using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Implementations.Endpoints;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Responses;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
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

        _contactEndpoint = new ContactEndpoint(
            _daktelaHttpClientMock.Object,
            new HttpResponseParser(_daktelaOptionsMock.Object),
            new PagedResponseProcessor<IContactEndpoint>()
        );
    }

    [Fact]
    public async Task GetSimpleContactWorks()
    {
        const string name = "testing_user";

        using var _ = _daktelaHttpClientMock.MockHttpGetResponse<Contact>(
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

        using var _ = _daktelaHttpClientMock.MockHttpGetResponse<Contact>(
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
    }

    [Fact]
    public async Task GetContactsWorks()
    {
        using var _ = _daktelaHttpClientMock.MockHttpGetListResponse<Contact>(
            $"{IContactEndpoint.UriPrefix}{IContactEndpoint.UriPostfix}", "contacts"
        );

        var responseMetadata = new TotalRecordsResponseMetadata();

        var count = 0;
        await foreach (
            var contact in _contactEndpoint.GetContactsAsync(
                RequestBuilder.CreateEmpty(),
                RequestOptionBuilder.CreateAutoPagingRequestOption(false),
                responseMetadata
            ))
        {
            count++;
            Assert.NotNull(contact);
        }

        Assert.Equal(2, count);
        var totalRecords = Assert.Single(responseMetadata.TotalRecords);
        Assert.Equal(2, totalRecords);
    }

    private class TotalRecordsResponseMetadata : ITotalRecordsResponseMetadata
    {
        public ICollection<int> TotalRecords { get; } = new List<int>();

        public void SetTotalRecords(int totalRecords) => TotalRecords.Add(totalRecords);
    }
}
