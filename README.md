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
  configuration.BaseUrl = "https://<domain>.daktela.com/api/";
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
    var responseMetadata = new TotalRecordsResponseMetadata();

    await foreach(
      var contact in _contactEndpoint.GetContactsAsync(
        RequestBuilder.CreatePaged(new Paging(0, 20)),
        RequestOptionBuilder.CreateAutoPagingRequestOption(false),
        responseMetadata
      )
    )
    {
      var contactName = contact.Name;
    }

    var totalRecords = responseMetadata.TotalRecords;
  }

  private class TotalRecordsResponseMetadata : ITotalRecordsResponseMetadata
  {
    public int? TotalRecords { get; private set; }

    public void SetTotalRecords(int totalRecords) => TotalRecords = totalRecords;
  }
}
```
