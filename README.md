[![Build Status](https://github.com/aviationexam/daktela-http-client/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/aviationexam/daktela-http-client/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Daktela.HttpClient.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Daktela.HttpClient/)
[![MyGet](https://img.shields.io/myget/daktela-http-client/vpre/Daktela.HttpClient?label=MyGet)](https://www.myget.org/feed/daktela-http-client/package/nuget/Daktela.HttpClient)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Faviationexam%2Freporting-api%2Fshield%Daktela.HttpClient%2Flatest&label=ReportingApi)](https://f.feedz.io/aviationexam/daktela-http-client/packages/Daktela.HttpClient/latest/download)

# Daktela Http Client

## How to configure library

Add library to the dependency container

```cs
using Daktela.HttpClient.Configuration;
using Daktela.HttpClient.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

IServiceCollection serviceCollection;

serviceCollection.AddDaktelaHttpClient((DaktelaOptions configuration) => {
  configuration.ApiDomain = "https://<domain>.daktela.com";
  configuration.AccessToken = "<AccessToken>";
  // HTTP request timeout
  configuration.Timeout = TimeSpan.FromSeconds(60);
  // Should match with timezone on the Daktela server
  configuration.DateTimeOffset = TimeSpan.FromMinutes(60);
});
```

## How to use library

```cs
using Daktela.HttpClient.Api.Contacts;
using Daktela.HttpClient.Api.Requests;
using Daktela.HttpClient.Implementations;
using Daktela.HttpClient.Interfaces.Endpoints;
using Daktela.HttpClient.Interfaces.Responses;
using System.Threading;
using System.Threading.Tasks;

public class MyService {
  private readonly IContactEndpoint _contactEndpoint;

  public MyService(IContactEndpoint contactEndpoint) {
    _contactEndpoint = contactEndpoint;
  }

  public async Task CallApiGet(CancellationToken cancellationToken = default) {
    Contact contact = await _contactEndpoint.GetContactAsync("<contactUniqueName>", cancellationToken);
  }

  public async Task CallApiGetList(CancellationToken cancellationToken = default)
  {
    var responseBehaviour = new TotalRecordsResponseBehaviour();
    // or you can simply use an empty response behavior as follow
    var emptyResponseBehaviour = ResponseBehaviourBuilder.CreateEmpty();

    await foreach(
      var contact in _contactEndpoint.GetContactsAsync(
        RequestBuilder.CreatePaged(new Paging(0, 20)),
        RequestOptionBuilder.CreateAutoPagingRequestOption(false),
        responseBehaviour,
        cancellationToken
      ).WithCancellation(cancellationToken)
    )
    {
      var contactName = contact.Name;
    }

    var totalRecords = responseBehaviour.TotalRecords;
  }

  private class TotalRecordsResponseBehaviour : ITotalRecordsResponseBehaviour
  {
    public int? TotalRecords { get; private set; }

    public void SetTotalRecords(int totalRecords) => TotalRecords = totalRecords;
  }
}
```
