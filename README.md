# .NET8-ASP.NET-Razor-Embedded-Database-E-commerce-Application
This is a fully functional online store project, complete with an administrative interface and a system for ratings and comments. It is distributed under an open license. The database used is the one built into Visual Studio.

Before using run following command in the Package Managment Console 

```sh
Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB; Database=OnlineDataBase;Trusted_Connection=True;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/Db -force
```
