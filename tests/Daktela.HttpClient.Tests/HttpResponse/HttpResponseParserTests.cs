using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Responses.Errors;
using Daktela.HttpClient.Api.Tickets;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Tests.Infrastructure;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Daktela.HttpClient.Tests.HttpResponse;

public class HttpResponseParserTests
{
    private readonly TimeSpan _dateTimeOffset = TimeSpan.FromMinutes(90);

    private readonly Mock<IOptions<DaktelaOptions>> _daktelaOptionsMock = new(MockBehavior.Strict);

    private readonly HttpJsonSerializerOptions _httpJsonSerializerOptions;

    public HttpResponseParserTests()
    {
        _daktelaOptionsMock.Setup(x => x.Value)
            .Returns(new DaktelaOptions
            {
                DateTimeOffset = _dateTimeOffset,
            });

        _httpJsonSerializerOptions = new HttpJsonSerializerOptions(_daktelaOptionsMock.Object);
    }

    [Fact]
    public async Task ParseContactsWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("contacts".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactsResponse = await httpResponseParser.ParseResponseAsync<ListResponse<ReadContact>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactsResponse.Error);
        Assert.NotNull(contactsResponse.Result);
        Assert.NotEmpty(contactsResponse.Result.Data);
        Assert.Equal(4, contactsResponse.Result.Total);
    }

    [Fact]
    public async Task ParseCreateContactBadEmailFormatRequestWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("create-contract-bad-email-format".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadContact>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.NotNull(contactResponse.Result);

        var result = contactResponse.Result;
        Assert.Equal("firstname lastname", result.Title);
        Assert.Equal("firstname", result.FirstName);
        Assert.Equal("lastname", result.LastName);
        Assert.Null(result.Account);
        Assert.Null(result.User);
        Assert.Null(result.Description);
        Assert.NotNull(result.CustomFields);
        Assert.Equal("name", result.Name);

        var error = Assert.IsType<ComplexErrorResponse>(contactResponse.Error);
        Assert.NotNull(error.Form);
        Assert.Null(error.Primary);

        var errorForm = Assert.Single(error.Form!);
        Assert.Equal("customFields", errorForm.Key);
        var nestedErrorForm = Assert.IsType<NestedErrorForm>(errorForm.Value);
        Assert.Single(nestedErrorForm.Keys, "email");
        var errorFormMessage = Assert.IsType<ErrorFormMessages>(Assert.Single(nestedErrorForm.Values));
        Assert.Single(errorFormMessage.ErrorMessages, "E-mail nen\u00ed ve spr\u00e1vn\u00e9m form\u00e1tu");
    }

    [Fact]
    public async Task ParseCreateContactBadRequestWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("create-contract-bad-request".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadContact>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.NotNull(contactResponse.Result);

        var result = contactResponse.Result;
        Assert.Equal("Title testing_user", result.Title);
        Assert.Equal("Title", result.FirstName);
        Assert.Equal("testing_user", result.LastName);
        Assert.Null(result.Account);
        Assert.Null(result.User);
        Assert.Null(result.Description);
        Assert.NotNull(result.CustomFields);
        Assert.Equal("testing_user", result.Name);

        var error = Assert.IsType<ComplexErrorResponse>(contactResponse.Error);
        Assert.NotNull(error.Form);
        Assert.Null(error.Primary);

        var errorForm = Assert.Single(error.Form!);
        Assert.Equal("user", errorForm.Key);
        var errorFormMessage = Assert.IsType<ErrorFormMessage>(errorForm.Value);
        Assert.Equal("Chyba cizího klíče", errorFormMessage.ErrorMessage);
    }

    [Fact]
    public async Task ParseErrorFormWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("error-form".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<object>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.Null(contactResponse.Result);

        var error = Assert.IsType<ComplexErrorResponse>(contactResponse.Error);
        Assert.NotNull(error.Form);
        Assert.Null(error.Primary);

        var errorForm = Assert.Single(error.Form!);
        Assert.Equal("lastname", errorForm.Key);
        var errorFormMessage = Assert.IsType<ErrorFormMessage>(errorForm.Value);
        Assert.Equal("Item is mandatory", errorFormMessage.ErrorMessage);
    }

    [Fact]
    public async Task ParseErrorPrimaryWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("error-primary".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<object>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.Null(contactResponse.Result);

        var error = Assert.IsType<ComplexErrorResponse>(contactResponse.Error);
        Assert.Null(error.Form);
        Assert.NotNull(error.Primary);

        Assert.Equal(2, error.Primary!.Count);
        Assert.True(new[]
        {
            "User have not available devices",
            "error",
        }.SequenceEqual(error.Primary.ToArray()));
    }

    [Fact]
    public async Task ParseErrorPrimaryAndFormWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("error-primary-and-form".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<object>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.Null(contactResponse.Result);

        var error = Assert.IsType<ComplexErrorResponse>(contactResponse.Error);
        Assert.NotNull(error.Form);
        Assert.NotNull(error.Primary);

        Assert.Equal(2, error.Primary!.Count);
        Assert.True(new[]
        {
            "User have not available devices",
            "error",
        }.SequenceEqual(error.Primary.ToArray()));

        var errorForm = Assert.Single(error.Form!);
        Assert.Equal("lastname", errorForm.Key);
        var errorFormMessage = Assert.IsType<ErrorFormMessage>(errorForm.Value);
        Assert.Equal("Item is mandatory", errorFormMessage.ErrorMessage);
    }

    [Fact]
    public async Task ParseErrorStringWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("error-string".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<object>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.Null(contactResponse.Result);

        var errors = Assert.IsType<PlainErrorResponse>(contactResponse.Error);

        var error = Assert.Single(errors);
        Assert.Equal("Object not exist", error);
    }

    [Fact]
    public async Task ParseErrorStringArrayWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("error-string-array".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<object>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.Null(contactResponse.Result);

        var errors = Assert.IsType<PlainErrorResponse>(contactResponse.Error);

        var error = Assert.Single(errors);
        Assert.Equal("Object not exist", error);
    }

    [Fact]
    public async Task ParseSimpleContactWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("simple-contact".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadContact>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.NotNull(contactResponse.Result);

        var contact = contactResponse.Result;

        Assert.NotNull(contact);
        Assert.Equal("testing_user", contact.Name);
        Assert.Equal(new DateTimeOffset(2022, 3, 2, 14, 6, 5, _dateTimeOffset), contact.Edited);
        Assert.Equal(new DateTimeOffset(2022, 3, 2, 14, 6, 5, _dateTimeOffset), contact.Created);
        Assert.Null(contact.User);
        Assert.Null(contact.Account);

        var errors = Assert.IsType<PlainErrorResponse>(contactResponse.Error);
        Assert.Empty(errors);
    }

    [Fact]
    public async Task ParseSimpleContactWithUserWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("simple-contact-with-user".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadContact>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.NotNull(contactResponse.Result);

        var contact = contactResponse.Result;

        Assert.NotNull(contact);
        Assert.Equal("test_user_with_user", contact.Name);
        Assert.Equal(new DateTimeOffset(2022, 3, 2, 15, 51, 17, _dateTimeOffset), contact.Created);
        Assert.Equal(new DateTimeOffset(2022, 3, 2, 16, 1, 27, _dateTimeOffset), contact.Edited);
        Assert.NotNull(contact.User);
        Assert.Null(contact.Account);

        Assert.Equal("administrator", contact.User!.Name);
        Assert.Equal("admin", contact.User.Role.Name);
        Assert.Equal("admin", contact.User.Profile.Name);

        var errors = Assert.IsType<PlainErrorResponse>(contactResponse.Error);
        Assert.Empty(errors);
    }

    [Fact]
    public async Task ParseUpdateContactWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("update-contact".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadContact>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.NotNull(contactResponse.Result);

        var contact = contactResponse.Result;

        Assert.NotNull(contact);
        Assert.Equal("testing_user_637828625039351324", contact.Name);
        Assert.Equal("Title testing_user_637828625039351324", contact.Title);
        Assert.Equal("Last testing_user_637828625039351324", contact.LastName);
        Assert.Equal(new DateTimeOffset(2022, 03, 14, 13, 48, 25, _dateTimeOffset), contact.Created);
        Assert.Equal(new DateTimeOffset(2022, 03, 14, 13, 48, 28, _dateTimeOffset), contact.Edited);
        Assert.Null(contact.User);
        Assert.Null(contact.Account);
        Assert.NotNull(contact.CustomFields);

        IDictionary<string, ICollection<string>> customFields = contact.CustomFields!;
        var customFieldsNumber = Assert.Contains("number", customFields);
        var customFieldsAddress = Assert.Contains("address", customFields);
        var customFieldsEmail = Assert.Contains("email", customFields);
        var customFieldsNote = Assert.Contains("note", customFields);

        var customFieldsNumberValue = Assert.Single(customFieldsNumber);
        Assert.Equal("123456789", customFieldsNumberValue);
        Assert.Empty(customFieldsAddress);
        var customFieldsEmailValue = Assert.Single(customFieldsEmail);
        Assert.Equal("my@email.com", customFieldsEmailValue);
        Assert.Empty(customFieldsNote);

        var error = Assert.IsType<PlainErrorResponse>(contactResponse.Error);
        Assert.Empty(error);
    }

    [Fact]
    public async Task ParseUpdateContactBadRequest()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("update-contact-bad-request".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadContact>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactResponse.Error);
        Assert.NotNull(contactResponse.Result);

        var contact = contactResponse.Result;

        Assert.NotNull(contact);
        Assert.Equal("testing_user_637828520552409483", contact.Name);
        Assert.Equal(new DateTimeOffset(2022, 3, 14, 10, 54, 16, _dateTimeOffset), contact.Created);
        Assert.Equal(new DateTimeOffset(2022, 3, 14, 10, 54, 30, _dateTimeOffset), contact.Edited);
        Assert.Null(contact.User);
        Assert.Null(contact.Account);

        var error = Assert.IsType<ComplexErrorResponse>(contactResponse.Error);
        Assert.NotNull(error.Form);
        Assert.Null(error.Primary);

        Assert.Equal(2, error.Form!.Count);
        var titleError = Assert.Contains("title", error.Form);
        var lastnameError = Assert.Contains("lastname", error.Form);

        var titleErrorMessage = Assert.IsType<ErrorFormMessage>(titleError);
        Assert.Equal("Vstup je povinn\u00fd", titleErrorMessage.ErrorMessage);
        var lastnameErrorMessage = Assert.IsType<ErrorFormMessage>(lastnameError);
        Assert.Equal("Vstup je povinn\u00fd", lastnameErrorMessage.ErrorMessage);
    }

    [Fact]
    public async Task ParseTicketWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("ticket-read".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var ticketResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadTicket>>(httpResponseContent, cancellationToken);

        Assert.NotNull(ticketResponse.Error);
        Assert.NotNull(ticketResponse.Result);

        var ticket = ticketResponse.Result;

        Assert.NotNull(ticket);
        Assert.Equal(9672, ticket.Name);
        Assert.Equal(EStage.Open, ticket.Stage);
        Assert.Equal(EPriority.Low, ticket.Priority);
        Assert.Equal(new DateTimeOffset(2022, 5, 4, 14, 39, 13, _dateTimeOffset), ticket.Created);
        Assert.Equal(new DateTimeOffset(2022, 5, 4, 14, 39, 13, _dateTimeOffset), ticket.Edited);
        Assert.Null(ticket.User);
        Assert.NotNull(ticket.Contact);
        Assert.NotNull(ticket.CustomFields);
        var customField = Assert.Single(ticket.CustomFields!);
        Assert.Equal("conversation_id", customField.Key);
        Assert.Equal("123", Assert.Single(customField.Value));

        var errors = Assert.IsType<PlainErrorResponse>(ticketResponse.Error);
        Assert.Empty(errors);
    }

    [Fact]
    public async Task ParseTicketBadRequest()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("ticket-read-bad-request".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var ticketResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadTicket>>(httpResponseContent, cancellationToken);

        Assert.NotNull(ticketResponse.Error);
        Assert.NotNull(ticketResponse.Result);

        var ticket = ticketResponse.Result;

        Assert.NotNull(ticket);
        Assert.Equal(9674, ticket.Name);
        Assert.Equal(EStage.Open, ticket.Stage);
        Assert.Equal(EPriority.Low, ticket.Priority);
        Assert.Equal(new DateTimeOffset(2022, 5, 4, 14, 57, 53, _dateTimeOffset), ticket.Created);
        Assert.Equal(new DateTimeOffset(2022, 5, 4, 14, 57, 53, _dateTimeOffset), ticket.Edited);
        Assert.Null(ticket.User);
        Assert.NotNull(ticket.Contact);

        var error = Assert.IsType<ComplexErrorResponse>(ticketResponse.Error);
        Assert.NotNull(error.Form);
        Assert.Null(error.Primary);

        Assert.Equal(1, error.Form!.Count);
        var titleError = Assert.Contains("title", error.Form);

        var titleErrorMessage = Assert.IsType<ErrorFormMessage>(titleError);
        Assert.Equal("Entry is required", titleErrorMessage.ErrorMessage);
    }

    [Fact]
    public async Task ParseTicketsWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("tickets-response".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var contactsResponse = await httpResponseParser.ParseResponseAsync<ListResponse<ReadTicket>>(httpResponseContent, cancellationToken);

        Assert.NotNull(contactsResponse.Error);
        Assert.NotNull(contactsResponse.Result);
        Assert.NotEmpty(contactsResponse.Result.Data);
        Assert.Equal(1, contactsResponse.Result.Total);
    }

    [Fact]
    public async Task ParseTicketActivityWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("ticket-activity-read".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var ticketActivityResponse = await httpResponseParser.ParseResponseAsync<SingleResponse<ReadActivity>>(httpResponseContent, cancellationToken);

        Assert.NotNull(ticketActivityResponse.Error);
        Assert.NotNull(ticketActivityResponse.Result);

        var ticketActivity = ticketActivityResponse.Result;

        Assert.NotNull(ticketActivity);
        Assert.Equal("activities-9673-637872782126648454", ticketActivity.Name);
        Assert.Equal(EAction.Close, ticketActivity.Action);
        Assert.Equal(0, ticketActivity.Priority);
        Assert.Equal(new DateTimeOffset(2022, 5, 4, 16, 23, 40, _dateTimeOffset), ticketActivity.TimeOpen);
        Assert.NotNull(ticketActivity.User);
        Assert.Null(ticketActivity.Contact);

        var errors = Assert.IsType<PlainErrorResponse>(ticketActivityResponse.Error);
        Assert.Empty(errors);
    }

    [Fact]
    public async Task ParseTicketActivitiesWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("ticket-activities-read".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var ticketActivitiesResponse = await httpResponseParser.ParseResponseAsync<ListResponse<ReadActivity>>(httpResponseContent, cancellationToken);

        Assert.NotNull(ticketActivitiesResponse.Error);
        Assert.NotNull(ticketActivitiesResponse.Result);
        Assert.NotEmpty(ticketActivitiesResponse.Result.Data);
        Assert.Equal(2, ticketActivitiesResponse.Result.Total);
    }

    [Fact]
    public async Task ParseTicketActivityAttachmentWorks()
    {
        var httpResponseParser = new HttpResponseParser(_httpJsonSerializerOptions);

        using var httpResponseContent = new StreamContent("read-activity-attachment".LoadEmbeddedJson());

        var cancellationToken = CancellationToken.None;
        var ticketActivityAttachmentResponse = await httpResponseParser.ParseResponseAsync<ListResponse<ReadActivityAttachment>>(httpResponseContent, cancellationToken);

        Assert.NotNull(ticketActivityAttachmentResponse.Error);
        Assert.NotNull(ticketActivityAttachmentResponse.Result);
        Assert.NotEmpty(ticketActivityAttachmentResponse.Result.Data);
        Assert.Equal(1, ticketActivityAttachmentResponse.Result.Total);
    }
}
