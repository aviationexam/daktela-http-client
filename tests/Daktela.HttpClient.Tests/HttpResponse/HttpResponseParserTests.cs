using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Api.Responses.Errors;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Tests.Infrastructure;
using Microsoft.Extensions.Options;
using Moq;
using System;
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
}
