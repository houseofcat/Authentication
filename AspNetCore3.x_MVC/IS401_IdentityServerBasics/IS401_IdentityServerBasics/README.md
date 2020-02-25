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
 * TestClient - WebApi
   * IHttpClientFactory
     * Named client usage.
     * Polly Plugin: Retry.
     * Polly Plugin: Circuit Breaker.

After AddingIdentityServer, the Apis, and the Clients. This document should respond if things are starting correctly.
```
https://localhost:5001/.well-known/openid-configuration  
```

Really notable mentions of using the `IdentityModel.Client` and the `IHttpClientFactory` with `Polly`.  

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
