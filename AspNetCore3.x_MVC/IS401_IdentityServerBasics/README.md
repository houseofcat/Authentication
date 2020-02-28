# Authentication - IS401 IdentityServerBasics

## Demonstrates

 * IdentityServer4 setup.  
 * Demonstrate protecting an API by using Client Credentials.  
 * Demonstrate defining Scopes (APIs) in the client.
 * WellKnown Configuration
 * DeveloperSignedCredentials (tempkey.RSA)
 * TestApi - WebApi
   * Setup for controllers.
   * Using JWT (AddJwtBearer() from pacakage)
   * Changing the Kestrel ports in `Properties/launchSettings.json`
   * Notes on ConfigureAwait / SyncContext
 * TestApiClient - WebApi
   * IHttpClientFactory
     * Named client usage.
     * Polly Plugin: Retry.
     * Polly Plugin: Circuit Breaker.

After AddingIdentityServer, the Apis, and the Clients. This document should respond if things are starting correctly.
```
https://localhost:5001/.well-known/openid-configuration  
```

An Example of the flow.

Postman/Curl/App  
```
HttpGet Request From TestApiClient against the TestApi URI/URL
https://localhost:5021/api/ClientAdmin/remote
```

```
TestApiClient Endpoint -> IdentityClient (using HttpClient #1) -> IdentityServer GetDiscoveryDoc    \\ Retrieve OpenId Configuration   
```

```
TestApiClient Endpoint -> IdentityClient (using HttpClient #1) -> IdentityServer GetClientToken     \\ Retrieve the ClientToken credential  
```

```
TestApiClient Endpoint -> Secure TestApi Endpoint w/ Token (using HttpClient #2)        \\ Action requiring Authorization  
```

```
Inbound
TestApi -> Validates Token w/ IdentityServer -> Perform Action       \\ Action is allowed.
```

```
Outbound
TestApi -> TestApiClient    \\ Return values.
```

Example Output to Postman/CurlApp
```json
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjRqR1JQMDAzLXlvR192RjR5Q2V3aXciLCJ0eXAiOiJhdCtqd3QifQ.eyJuYmYiOjE1ODI2NzQ4OTEsImV4cCI6MTU4MjY3ODQ5MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6IlRlc3RBcGkiLCJjbGllbnRfaWQiOiJUZXN0Q2xpZW50Iiwic2NvcGUiOlsiVGVzdEFwaSJdfQ.xuwGGPXzeeleBxqj4YlKYj9pZANmL3XXt74bEgiyEUeQv-kaYN7Pu1eKaj7XfM0y2pLENZuzU2bWTA-zcN0NZZPJqrzXOKfaztSzut2lAhMTQsWCVv9YLY7wj_VNMg1M3ytqcEuDtlbOKByIA95i7R8dLhMrcNzXtVah5Mqan5b2xMKRV__Ghm7RW9mhbaMBtWZ_Pe13AexuJBPTpFl0Il_T7JDlhmvWPVlNOzzD6d4nhw9J9zlFrwQNhIwBH_n0Yu8CNzDc405U6rGE4uliRnpU0-AItF4KpkT66etBjylx6rAJR-Zfzor7wwjkSoDni1aJlOK4uS6WxZD70qfUyw",
    "message": "Action performed successfully."
}
```

Generally speaking, you could remove the OpenId config step but if you hardcode these values you will need to update code when/if it ever changes URL/URI or Domain etc.
If you do retrieve the doc, it is also probably best to cache this so you don't get it on every call.

Really notable mentions in this demo are the using of `IdentityModel.Client` which adds HttpClient extensions and the `IHttpClientFactory` with `Polly` for HttpClient management, retry policies, and circuit breaker pattern.  

Here are the polly plugin policies being used:  

```csharp
public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
```

## Sources

IdentityServer 4 Protecting an API using Client Credentials  
https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html  

Stephen Cleary's Blog - ASP.NET Core SynchronizationContext  
https://blog.stephencleary.com/2017/03/aspnetcore-synchronization-context.html  

Microsoft Doc - Make HTTP requests using IHttpClientFactory in ASP.NET Core  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1#consumption-patterns  

Microsoft Doc - Use HttpClientFactory to implement resilient HTTP requests  
https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#multiple-ways-to-use-httpclientfactory  

Polly - ThePollyProject.org  
https://github.com/App-vNext/Polly  
http://www.thepollyproject.org/  
https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory  

Microsoft Doc - Implement HTTP call retries with exponential backoff with HttpClientFactory and Polly policies  
https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly  

Microsoft Doc - Implement the Circuit Breaker pattern  
https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-circuit-breaker-pattern  

Identity Tutorial - Raw Coding Youtube Guide (This is just taught really well)   
https://www.youtube.com/watch?v=jARHHUsljeo  
