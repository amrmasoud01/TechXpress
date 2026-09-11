# TechXpress

[![Build](https://github.com/amrmasoud01/TechXpress/actions/workflows/ci.yml/badge.svg)](https://github.com/amrmasoud01/TechXpress/actions/workflows/ci.yml)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4)](https://learn.microsoft.com/aspnet/core/mvc/)
[![Live Demo](https://img.shields.io/badge/Live_Demo-Open-2EA44F?logo=googlechrome&logoColor=white)](https://techxpress0.runasp.net/)

TechXpress is an ASP.NET Core MVC e-commerce application for browsing electronics, managing a cart and wishlist, placing Stripe Checkout orders, and administering products, categories, users, roles, and order reports.

**Live demo:** [techxpress0.runasp.net](https://techxpress0.runasp.net/)

## Highlights

- Product search, category filters, sorting, and pagination
- Product details, stock tracking, ratings, and reviews
- Session-based shopping cart and wishlist
- ASP.NET Core Identity authentication and role-based authorization
- Stripe Checkout integration with server-side payment verification
- Admin dashboard for catalog, users, roles, orders, and PDF reports
- Repository and Unit of Work patterns across a three-layer architecture

## Architecture

```text
techXpress.UI          MVC controllers, Razor views, view models
techXpress.Services    Business logic, DTOs, file and payment services
techXpress.DataAccess  EF Core entities, repositories, migrations
```

## Technology

- .NET 9 and ASP.NET Core MVC
- Entity Framework Core 9 and SQL Server
- ASP.NET Core Identity
- Stripe.net
- Bootstrap, jQuery, DataTables, and Chart.js
- Rotativa/wkhtmltopdf for PDF reports

## Local setup

Requirements:

- .NET 9 SDK
- SQL Server or SQL Server LocalDB
- A Stripe test account for checkout

Restore the tools and packages from the repository root:

```powershell
dotnet restore techXpress.UI/techXpress.sln
dotnet tool restore --tool-manifest techXpress.UI/.config/dotnet-tools.json
```

The default connection uses SQL Server LocalDB. To use another SQL Server instance, set an environment variable instead of putting credentials in source control:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=YOUR_SERVER;Database=TechXpress;User Id=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True"
```

Add Stripe test keys in the same way:

```powershell
$env:Stripe__SecretKey = "sk_test_..."
$env:Stripe__PublishableKey = "pk_test_..."
```

Create the database and start the application:

```powershell
dotnet ef database update --project techXpress.DataAccess --startup-project techXpress.UI
dotnet run --project techXpress.UI
```

The application creates the `Admin`, `Seller`, and `Customer` roles on first start. To create an initial administrator, set these variables before the first run:

```powershell
$env:SeedAdmin__Email = "admin@example.com"
$env:SeedAdmin__Password = "Choose-A-Strong-Password-123!"
$env:SeedAdmin__UserName = "admin"
```

Remove the `SeedAdmin` variables after the administrator has been created.

## Configuration

| Setting | Purpose |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | SQL Server connection string |
| `Stripe__SecretKey` | Stripe secret test or live key |
| `Stripe__PublishableKey` | Stripe publishable key |
| `SeedAdmin__Email` | Optional initial administrator email |
| `SeedAdmin__Password` | Optional initial administrator password |
| `SeedAdmin__UserName` | Optional initial administrator display name |

Do not commit production connection strings, Stripe keys, or administrator passwords.

## Build

```powershell
dotnet build techXpress.UI/techXpress.sln --configuration Release
```

GitHub Actions runs the same Release build for pushes and pull requests.
