this application uses the following nuget packages:

Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.AspNetCore.Identity.UI
Microsoft.AspNetCore.OpenApi
Microsoft.EntityFrameworkCore.Sqlite
Microsoft.EntityFrameworkCore.Tools
Microsoft.VisualStudio.Web.CodeGeneration.Design
NETCore.MailKit
Swashbuckle.AspNetCore.Swagger
Swashbuckle.AspNetCore.SwaggerGen
Swashbuckle.AspNetCore.SwaggerUI
AspnetCore.Unobtrusive.Ajax
EPPlus
QRCoder

## How to run the application
1. Open this project in Visual Studio
2. Open the Package Manager Console
3. Run the following command to create the database:
```
Update-Database
```
4. Run the application
If all goes well, an automatic browser window will open with the application running. If not, you can access the application through a Localhost URL(localhost:7085).

You can also navigate to the database used by the application by opening the Database Explorer in Visual Studio.

IF YOU ARE PLANNING TO READ AN EXCEL FILE:
Your file must have a series of objects, divided into the following columns:
1. Type
2. Name
3. Serial Number

the type column must contain the type of object, the name column must contain the name of the object and the serial number column must contain the serial number of the object.
these are located in the first, second and third columns of a fresh Excel sheet respectively.

***AZURE FEATURES***
The application has been configured to use Azure for microsoft account login. If you want to use this feature, you must configure the application in Azure. To do this, follow the steps below:
1. Go to the Azure portal
2. Browse to 'ReservationApp' and click on 'Authentication'
3. Find the following redirect URI:
```
https://localhost:7085/signin-oidc
```
you can use a different redirect URI if you want. Just make sure to change the redirect URI in the application wherever needed.
4. Copy the Application ID and Tenant ID of the application you just created
5. Open the appsettings.json file in the project
6. Change the following values to the values you copied from Azure:
```
"ClientId":
"TenantId":
```
7. Save the file and run the application

You should now be able to login using your Microsoft account.

Also remember to change the redirect URI in the Azure application to the URL of the application you are running once you deploy it.









