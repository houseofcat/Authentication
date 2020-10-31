# Authentication
Creating functioning Authentication by example for AspNetCore. These are primarily for learning. They aren't quite ready for production but it should be great prototypes to help you get 90% of the way there.

I have tried to include the very sources I came across. Anything I used to help try and refresh my memory, or solve NetCore 3.x variations, or just best practices I wasn't aware of.

The projects were designed to be working locally (Kestrel), some issues will occur on specific projects I have made notes of.  


## AspNetCore3.x_IS4.1
### IS01_IdentityServer4.1_AspNetIdentity
Analyzing the new raw template and tweaking it as needed in line with the other older projects.


## AspNetCore3.x_MVC (Still useful but missing IS4.1 changes)
### C01_CookieBasic  
Project for working out a basic Cookie authentication with AspNetCore.  

[More Details in Readme](https://github.com/houseofcat/Authentication/blob/master/AspNetCore3.x_MVC/C01_CookieBasics/README.md)  

### C02_CookieAuthPolicy  
Project for using cookies, claims, and roles, to authenticate properly.  

[More Details in Readme](https://github.com/houseofcat/Authentication/tree/master/AspNetCore3.x_MVC/C02_CookieAuthPolicy)  

### I01_BasicIdentity  
Project for working out a basic Cookie authentication with AspNetCore, EntityFramework Core, and AspNetCore Identity.  

[More Details in Readme](https://github.com/houseofcat/Authentication/blob/master/AspNetCore3.x_MVC/I01_IdentityBasics/README.md)  

### I02_IdentityEmailConfirm  
A continuation of I01_BasicIdentity but adding in Email functionality from AspNetCore Identity and MailKit.  

[More Details in Readme](https://github.com/houseofcat/Authentication/blob/master/AspNetCore3.x_MVC/I02_IdentityEmailConfirm/README.md)   

### I03_IdentityResetPassword  
A continuation of I02_IdentityEmailConfirm, making structural changes to the code, adding ForgotPassword and ResetPassword functionality.  

[More Details in Readme](https://https://github.com/houseofcat/Authentication/blob/master/AspNetCore3.x_MVC/I03_IdentityResetPassword/README.md)  

### IS401_IdentityServerBasics 
A new project to show how to start using IdentityServer4.  

[More Details in Readme](https://github.com/houseofcat/Authentication/blob/master/AspNetCore3.x_MVC/IS401_IdentityServerBasics/)  

### IS402_IdentityServerMvcClient  
A continuation of IS401_IdentityServerBasics to show how to start using IdentityServer4 with OpenId (but stops at the login.)  

[More Details in Readme](https://github.com/houseofcat/Authentication/blob/master/AspNetCore3.x_MVC/IS402_IdentityServerMvcClient/)  

### IS403_IdentityServer_AspNetIdentity_SqlServer  
A continuation of IS402_IdentityServerMvcClient to show how to setup Serilog, AspNetIdentity, and SqlServer with IdentityServer4.  

[More Details in Readme](https://github.com/houseofcat/Authentication/blob/master/AspNetCore3.x_MVC/IS403_IdentityServer_AspNetIdentity_SqlServer/)  

### IS404_IdentityServer_AspNetIdentity_CustomUser  
A continuation of IS403_IdentityServer_AspNetIdentity_SqlServer with prettier logging, custom IdentityUser/IdentityRole, change primary key type, and more!  

[More Details in Readme](https://github.com/houseofcat/Authentication/tree/master/AspNetCore3.x_MVC/IS404_IdentityServer_AspNetIdentity_CustomUser/)  

### IS405_IdentityServer_AspNetIdentity_StoredProcedures  
A continuation of IS404_IdentityServer_AspNetIdentity_CustomUser with replacing some EntityFramework with SqlServer stored procedures.  

[More Details in Readme](https://github.com/houseofcat/Authentication/tree/master/AspNetCore3.x_MVC/IS405_IdentityServer_AspNetIdentity_StoredProcedures/)  

### IS406_IdentityServer  
A focus back on IdentityServer (with AspNetIdentity, SqlServer, and Serilog) authentication and authorization.  

[More Details in Readme](https://github.com/houseofcat/Authentication/tree/master/AspNetCore3.x_MVC/IS406_IdentityServer/)  

### IS407_IdentityServerContinued   
A continuation of IS406_IdentityServer continuing authentication and authorization.   

[More Details in Readme](https://github.com/houseofcat/Authentication/tree/master/AspNetCore3.x_MVC/IS407_IdentityServerContinued/)  