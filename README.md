 RealEstate API

## Overview
Property Search API built as part of a backend engineering take-home assignment.  
Allows CRUD operations and searching of real-estate properties and their spaces (rooms).

## Overview Notes
Language and framework choices because position is for a C# backend engineer role, so I used C# and .NET
I prefer stable and long time support versions of the .NET framework so I use .NET 8
Includes data-access patterns such as UnitofWork/MS SQL Server etc. and other enterprise design practices,
as these were explicitly highlighted as areas of interest by the technical reviewers.

## Tech Stack

- **Language:** C# (.NET 8)
- **API project:** `RealEstate.Api`
- **Web Framework:** ASP.NET Core 8
- **Testing Framework:** xUnit
- **Database:** MS SQL Server
- **Architecture:** Clean Architecture with separated Domain (DDD) and Infrastructure layers (Api/BLL/DAL/Domain/Tests)
- **ORM:** Entity Framework Core (8)
- **Dependency Injection:** Built-in ASP.NET Core DI
- **Logging:** Built-in ASP.NET Core logging
- **Documentation:** OpenAPI (Swagger) for API endpoints
- **Data Seeding:** EF Core Code firts migrations for database and initial data seeding use DatabaseInitializer


## Getting Started

1.Clone repository
2.Set the `"DefaultConnection"` in `RealEstate.Api/appsettings.json`
3.Run solution with startup project: `RealEstate.Api` 


> ⚙️ On first run, the project will automatically:
> - Create the **RealEstate** database (EF migrations)
> - Seed initial **sample data** (properties and spaces)

## Endpoints

| Method | URL                     | Description                                             |
|-------:|-------------------------|---------------------------------------------------------|
| GET    | `/properties`          | List properties — supports `type`, `min_price`, `max_price`, pagination (`page`, `limit`) and sorting (`sort=price_asc`/`price_desc`)				 |
| GET    | `/properties/{id}`     | Get property by ID with nested spaces                    |
| POST   | `/properties`          | Create new property (optionally with spaces)             |
| GET    | `/spaces`             | List spaces filtered by `property_id`, `type`, `min_size` |
| GET    | `/stats`              | Aggregated stats											 |


## Tests

Project includes:

- **Unit tests** (`RealEstate.Tests/UnitPropertiesControllerTests.cs`)
- **Integration tests** (`IntegrationPropertiesTests.cs`)

Run via **Test Explorer** in Visual Studio.


## Notes on Caching / Scalability
To improve performance under high load — caching layers such as **Redis** (or managed solutions in Azure/AWS) 
are recommended for endpoints like `/properties`, `/spaces`, and `/stats` to reduce database pressure. 
Additional scaling can be achieved by introducing API Gateway + horizontal scaling of the API.
