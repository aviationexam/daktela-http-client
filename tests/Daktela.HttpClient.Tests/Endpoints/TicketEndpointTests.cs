using Daktela.HttpClient.Api;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Implementations.Endpoints;
using Daktela.HttpClient.Interfaces;
using Daktela.HttpClient.Interfaces.Endpoints;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.Endpoints;

public class TicketEndpointTests
{
    private readonly TimeSpan _dateTimeOffset = TimeSpan.FromMinutes(90);

    private readonly Mock<IDaktelaHttpClient> _daktelaHttpClientMock = new(MockBehavior.Strict);

    private readonly ITicketEndpoint _ticketEndpoint;

    public TicketEndpointTests()
    {
        DaktelaJsonSerializerContext.SerializationDateTimeOffset = _dateTimeOffset;

        _ticketEndpoint = new TicketEndpoint(
            _daktelaHttpClientMock.Object,
            new HttpRequestSerializer(),
            new HttpResponseParser(),
            new PagedResponseProcessor<ITicketEndpoint>()
        );
    }

    [Fact]
    public async Task GetTicketWorks()
    {
        const int name = 1;

        using var _ = _daktelaHttpClientMock.MockHttpGetResponse<ReadTicket>(
            $"{ITicketEndpoint.UriPrefix}/{name}{ITicketEndpoint.UriPostfix}", "simple-ticket-response"
        );
        var ticket = await _ticketEndpoint.GetTicketAsync(name);

        Assert.NotNull(ticket);
        Assert.NotNull(ticket.User);
        Assert.Equal(name, ticket.Name);
        Assert.Equal(new DateTimeOffset(2021, 12, 10, 16, 50, 19, _dateTimeOffset), ticket.Edited);
        Assert.Equal(new DateTimeOffset(2021, 12, 10, 16, 50, 19, _dateTimeOffset), ticket.Created);
        Assert.Equal("Customer Support", ticket.Category.Title);
        Assert.Equal("Administrator", ticket.User!.Title);
        Assert.Equal(EStage.Open, ticket.Stage);
        Assert.NotNull(ticket.Statuses);
    }

    [Fact]
    public async Task DeleteTicketsWorks()
    {
        const int name = 1;

        _daktelaHttpClientMock.MockHttpDeleteResponse(
            $"{ITicketEndpoint.UriPrefix}/{name}{ITicketEndpoint.UriPostfix}"
        );

        await _ticketEndpoint.DeleteTicketAsync(name);
    }
}
