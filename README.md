# Esperancilla_WebAPI

An ASP.NET Core Web API (.NET 8) that manages products in SQL Server with Entity Framework Core. It has Swagger UI for testing.

## Project structure

```
Esperancilla_WebAPI.sln
Esperancilla_WebAPI/
├── Controllers/
│   └── ProductController.cs   # CRUD + search endpoints
├── Data/
│   └── AppDbContext.cs        # EF Core DbContext (Products table)
├── Model/
│   └── Product.cs             # Product entity
├── Properties/
│   └── launchSettings.json
├── appsettings.json           # Connection string
├── Esperancilla_WebAPI.http   # Sample requests for every endpoint
└── Program.cs                 # DbContext registration, Swagger, controllers
```

NuGet packages:
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Swashbuckle.AspNetCore` (Swagger)

## Setup

1. Open `Esperancilla_WebAPI.sln` in Visual Studio 2022.
2. In `Esperancilla_WebAPI/appsettings.json`, check that `ConnectionStrings:DefaultConnection` points to your SQL Server instance:
   ```json
   "DefaultConnection": "Server=BENJO\\MSSQLSERVER02;Database=MyFirstDatabaseApi;User Id=sa;Password=password;Trusted_Connection=True;TrustServerCertificate=True;"
   ```
   > In JSON the backslash in the instance name has to be escaped (`\\`).
   > With `Trusted_Connection=True` the app signs in with Windows authentication and ignores `User Id` and `Password`. To sign in as `sa` instead, remove `Trusted_Connection=True`.
3. Open **Tools > NuGet Package Manager > Package Manager Console** and run:
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```
   This creates the `MyFirstDatabaseApi` database with a `Products` table.

   (With the .NET CLI instead: `dotnet ef migrations add InitialCreate` and `dotnet ef database update`, run from the `Esperancilla_WebAPI` folder.)
4. Press **F5**. The browser opens Swagger UI at `https://localhost:<port>/swagger`.

## Endpoints

| Method | Route | Description | Responses |
|--------|-------|-------------|-----------|
| GET | `/api/Product` | Gets all products (`ToListAsync`) | 200 with an array |
| POST | `/api/Product` | Creates a product (`Add` + `SaveChangesAsync`) | 200 with the saved product and its new `Id` |
| GET | `/api/Product/{id}` | Gets one product by primary key (`FindAsync`) | 200 / 404 |
| GET | `/api/Product/search/{name}` | Gets the **first** product whose name contains `name` | 200 with one object / 404 |
| PUT | `/api/Product/{id}` | Updates `Name`, `Price` and `Stock` | 200 with the updated product / 404 |
| DELETE | `/api/Product/{id}` | Deletes a product | 200 `"Product deleted successfully!"` / 404 |
| GET | `/api/Product/SearchPartialProductName/{name}` | Gets **all** products matching `%name%` (`EF.Functions.Like`) | 200 with an array / 404 if none match |

Example POST body:

```json
{ "name": "Mouse", "price": 25.00, "stock": 10 }
```

Every endpoint has a ready-to-send request in `Esperancilla_WebAPI/Esperancilla_WebAPI.http`. Visual Studio can send them from the editor.

## Troubleshooting

### `Update-Database` fails with "You must install or update .NET to run this application"

The project targets .NET 8, and the EF Core tools start on the project's runtime. If the PC only has a newer runtime installed (for example .NET 10), the tools can't find .NET 8 and stop with this error.

The project file sets `<RollForward>Major</RollForward>`, so the tools and the app use the newer runtime instead. If you still see the error:
- Add that line inside `<PropertyGroup>` in your `.csproj`, **Rebuild**, and run `Update-Database` again, **or**
- install the [.NET 8 runtime (x64)](https://dotnet.microsoft.com/download/dotnet/8.0).
