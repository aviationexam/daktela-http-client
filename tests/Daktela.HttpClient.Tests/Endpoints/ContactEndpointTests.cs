using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Responses;
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IO;
using System.Net.Http;
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

        _contactEndpoint = new ContactEndpoint(
            _daktelaHttpClientMock.Object,
            new HttpResponseParser(_daktelaOptionsMock.Object)
        );
    }

    [Fact]
    public async Task GetSimpleContactWorks()
    {
        const string name = "testing_user";

        using var _ = await _daktelaHttpClientMock.MockHttpGetResponse<Contact>(
            $"{IContactEndpoint.UriPrefix}/{name}{IContactEndpoint.UriPostfix}",
            @"
{
  ""error"": [],
  ""result"": {
    ""title"": ""Testing user"",
    ""firstname"": ""Testing"",
    ""lastname"": ""user"",
    ""account"": null,
    ""user"": null,
    ""description"": """",
    ""nps_score"": 0,
    ""edited"": ""2022-03-02 14:06:05"",
    ""created"": ""2022-03-02 14:06:05"",
    ""customFields"": {
      ""number"": [],
      ""address"": [],
      ""email"": [],
      ""note"": []
    },
    ""duplicateContacts"": null,
    ""notDuplicateContacts"": null,
    ""name"": ""testing_user"",
    ""_sys"": {
      ""hash"": ""87"",
      ""model"": ""contacts"",
      ""cfScheme"": {
        ""number"": {
          ""type"": ""PHONE"",
          ""title"": ""Telefon"",
          ""hidden"": false,
          ""readonly"": false
        },
        ""address"": {
          ""type"": ""ADDRESS"",
          ""title"": ""Adresa"",
          ""hidden"": false,
          ""readonly"": false
        },
        ""email"": {
          ""type"": ""EMAIL"",
          ""title"": ""Email"",
          ""hidden"": false,
          ""readonly"": false
        },
        ""note"": {
          ""type"": ""TEXTAREA"",
          ""title"": ""Poznámka"",
          ""hidden"": false,
          ""readonly"": false
        }
      }
    }
  },
  ""_time"": ""2022-03-02 14:06:16""
}
");

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

        using var _ = await _daktelaHttpClientMock.MockHttpGetResponse<Contact>(
            $"{IContactEndpoint.UriPrefix}/{name}{IContactEndpoint.UriPostfix}",
            @"
{
  ""error"": [],
  ""result"": {
    ""title"": ""Test user with user"",
    ""firstname"": ""Test"",
    ""lastname"": ""User"",
    ""account"": null,
    ""user"": {
      ""title"": ""Administrator"",
      ""alias"": """",
      ""profile"": {
        ""title"": ""Administrator"",
        ""description"": """",
        ""maxActivities"": null,
        ""maxOutRecords"": 5,
        ""deleteMissedActivity"": false,
        ""noQueueCallsAllowed"": false,
        ""canTransferCall"": ""blind and assisted transfer"",
        ""options"": {
          ""hdColumns"": {
            ""user"": [],
            ""stage"": [],
            ""edited"": [],
            ""contact"": [],
            ""category"": [],
            ""priority"": [],
            ""statuses"": [],
            ""edited_by"": [],
            ""sla_deadtime"": []
          },
          ""extensionRequired"": false
        },
        ""customViews"": {
          ""views_56cb1b296f52b107759669"": {
            ""index"": 0,
            ""config"": {
              ""unwrap"": false
            },
            ""children"": []
          },
          ""views_56cb1b3b23717310986148"": {
            ""index"": 1,
            ""config"": {
              ""unwrap"": false
            },
            ""children"": []
          }
        },
        ""name"": ""admin"",
        ""_sys"": {
          ""hash"": ""450"",
          ""model"": ""profiles""
        }
      },
      ""role"": {
        ""title"": ""Administrator"",
        ""description"": """",
        ""options"": {
          ""tabs"": 31,
          ""pFbms"": 0,
          ""pVbrs"": 0,
          ""pWaps"": 0,
          ""roles"": 31,
          ""users"": 31,
          ""events"": 2,
          ""export"": 32,
          ""groups"": 31,
          ""pCalls"": 752,
          ""pChats"": 0,
          ""pSmses"": 0,
          ""pStats"": 16,
          ""pauses"": 31,
          ""queues"": 31,
          ""pEmails"": 16,
          ""qaforms"": 2,
          ""reports"": 31,
          ""tickets"": 31,
          ""accounts"": 31,
          ""articles"": 0,
          ""contacts"": 31,
          ""pTracing"": 16,
          ""profiles"": 31,
          ""sessions"": 16,
          ""statuses"": 31,
          ""analytics"": 0,
          ""databases"": 15,
          ""pAttempts"": 0,
          ""pRealtime"": 48,
          ""qaReviews"": 0,
          ""templates"": 31,
          ""blackLists"": 2,
          ""crmRecords"": 31,
          ""extensions"": 31,
          ""formFields"": 15,
          ""gdprPolicy"": 0,
          ""pCallviews"": 0,
          ""pFaxserver"": 0,
          ""pGreetings"": 48,
          ""pSmsserver"": 0,
          ""ticketsSla"": 31,
          ""timegroups"": 31,
          ""wallboards"": 2,
          ""pActivities"": 16,
          ""articlesTags"": 31,
          ""pFbmRoutings"": 18,
          ""pSmsRoutings"": 18,
          ""pVbrRoutings"": 18,
          ""pWapRoutings"": 18,
          ""ticketsRules"": 31,
          ""ticketsViews"": 31,
          ""formCrmFields"": 15,
          ""pIntegrations"": 16,
          ""pProvisioning"": 16,
          ""ticketsMacros"": 31,
          ""campaignsTypes"": 31,
          ""extensions_ext"": 31,
          ""externalsUsers"": 0,
          ""ftpsRecordings"": 0,
          ""pCallsRoutings"": 16,
          ""pChatsRoutings"": 18,
          ""articlesFolders"": 31,
          ""crmRecordsTypes"": 31,
          ""pAccountsScheme"": 16,
          ""pContactsScheme"": 16,
          ""pGlobalsettings"": 16,
          ""_setDependencies"": true,
          ""analyticsMetrics"": 0,
          ""articlesComments"": 23,
          ""campaignsRecords"": 0,
          ""forwardingNumber"": 0,
          ""ticketsCategories"": 31,
          ""extensions_msteams"": 0,
          ""systemAnnouncements"": 31
        },
        ""shortcuts"": [
          {
            ""icon"": ""fas fa-search"",
            ""type"": ""search"",
            ""color"": ""#428bca""
          },
          {
            ""icon"": ""fas fa-book"",
            ""type"": ""link"",
            ""color"": ""#d15b47"",
            ""title"": ""New CRM contact"",
            ""options"": {
              ""href"": ""/crm/contacts/create""
            }
          },
          {
            ""icon"": ""fas fa-tasks"",
            ""type"": ""link"",
            ""color"": ""#ffb752"",
            ""title"": ""List of all activites"",
            ""options"": {
              ""href"": ""/listing/activities/""
            }
          },
          {
            ""icon"": ""fas fa-question-circle"",
            ""type"": ""docs"",
            ""color"": ""#a0a0a0"",
            ""options"": {
              ""href"": ""https://doc.daktela.com/"",
              ""targetBlank"": true
            }
          }
        ],
        ""name"": ""admin"",
        ""_sys"": {
          ""hash"": ""36"",
          ""model"": ""roles""
        }
      },
      ""description"": """",
      ""call_steering_description"": """",
      ""extension"": ""500"",
      ""acl"": null,
      ""extension_state"": ""offline"",
      ""clid"": """",
      ""static"": false,
      ""recordAtCallStart"": ""disabled"",
      ""algo"": null,
      ""email"": """",
      ""emailAuth"": """",
      ""icon"": """",
      ""emoji"": """",
      ""options"": {
        ""sign"": """",
        ""target"": ""terminate|busy"",
        ""ad_password_update"": 1644312647,
        ""last_password_update"": 1636724642
      },
      ""nps_score"": 0,
      ""backoffice_user"": false,
      ""forwarding_number"": """",
      ""deleted"": false,
      ""deactivated"": false,
      ""allowRecordingInterruption"": false,
      ""name"": ""administrator"",
      ""_sys"": {
        ""hash"": null,
        ""model"": ""users""
      }
    },
    ""description"": """",
    ""nps_score"": 0,
    ""edited"": ""2022-03-02 16:01:27"",
    ""created"": ""2022-03-02 15:51:17"",
    ""customFields"": {
      ""number"": [],
      ""address"": [],
      ""email"": [],
      ""note"": []
    },
    ""duplicateContacts"": null,
    ""notDuplicateContacts"": null,
    ""name"": ""test_user_with_user"",
    ""_sys"": {
      ""hash"": ""90"",
      ""model"": ""contacts"",
      ""cfScheme"": {
        ""number"": {
          ""type"": ""PHONE"",
          ""title"": ""Telefon"",
          ""hidden"": false,
          ""readonly"": false
        },
        ""address"": {
          ""type"": ""ADDRESS"",
          ""title"": ""Adresa"",
          ""hidden"": false,
          ""readonly"": false
        },
        ""email"": {
          ""type"": ""EMAIL"",
          ""title"": ""Email"",
          ""hidden"": false,
          ""readonly"": false
        },
        ""note"": {
          ""type"": ""TEXTAREA"",
          ""title"": ""Poznámka"",
          ""hidden"": false,
          ""readonly"": false
        }
      }
    }
  },
  ""_time"": ""2022-03-02 18:36:46""
}
");

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

    private async Task<IDisposable> MockHttpGetResponse<THttpContract, TContract>(string name, string httpContent)
        where THttpContract : class
        where TContract : class
    {
        // disposed from httpResponseContent
        var memoryStream = new MemoryStream();

        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await streamWriter.WriteAsync(httpContent);
        await streamWriter.FlushAsync();
        streamWriter.Close();

        memoryStream.Seek(0, SeekOrigin.Begin);

        var httpResponseContent = new StreamContent(memoryStream);

        _daktelaHttpClientMock.Setup(x => x.GetAsync<TContract>(
            It.IsAny<IHttpResponseParser>(),
            name,
            It.IsAny<CancellationToken>()
        )).Returns((
            IHttpResponseParser httpResponseParser, string _, CancellationToken cancellationToken
        ) => httpResponseParser.ParseResponseAsync<THttpContract>(httpResponseContent, cancellationToken));

        return httpResponseContent;
    }
}
