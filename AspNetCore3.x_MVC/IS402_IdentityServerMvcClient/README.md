# Authentication - IS402 IdentityServerMvcClient

## Demonstrates

 * All of IS401_IdentityServerBasics
 * Additional client that is a GrantType.Code
 * An ApiResource that is also a client.
 * Using OpenIdConnect
 * Adding IdentityResources to IdentityServer.
   * Modifying the new Client to include these new resources as scopes.

## Note
This demonstrates authorize actions involving redirect. It stops right at the login point, having been redirected  
and scope validated against IdentityServer4. The login portion is in the IS403_IdentityServerWithAspNetIdentity.  

Starting all IdentityServer4 and TestMvcClient and navigating to the /home/profile page should trigger  
redirects which you can view in the Kestrel output or Chrome Debugging.  

## Sources

OIDC Specification  
https://openid.net/specs/openid-connect-core-1_0.html  

Application types for Microsoft identity platform  
https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-app-types  

Microsoft Doc - Microsoft identity platform and OpenID Connect protocol  
https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc  

Microsoft Doc - OAuth 2.0 and OpenID Connect protocols on the Microsoft identity platform  
https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-protocols  

Identity Tutorial - Raw Coding Youtube Guide (This is just taught really well)  
https://www.youtube.com/watch?v=8zupoD2pzZY  
