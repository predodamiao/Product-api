# Product API

API developed to manage product information.

## Technologies

- ASP.NET 8
- PostgreSQL 16
- MemoryCache
- FluentResult
- FluentValidation
- Entity Framework

## Como Rodar

| There is already an database configured in the project, but if you want to create a new one, remember to update the connection string in the appsettings files and run migrations.

1. Clone the repository:

```bash
git clone https://github.com/predodamiao/Product-api
cd Product-api
```

2. Build and Run the API with cli:

```bash
dotnet run
```

3. Access the API at [http://localhost:80](http://localhost:80).

## Database Creation Strategy

The database is created using Entity Framework Core's "Code First" approach. This means that the database is automatically generated based on the domain models defined in the project.

## Migrations

### How to create

To create a new migration, run the following commands:

```bash
dotnet ef migrations add <migration name> --project .\Infrastructure --startup-project .\Api -o .\Database\Migrations
```

### How to run

To run the migrations and create the database, run the following commands:

```bash
dotnet ef database update --project .\Infrastructure --startup-project .\Api
```

## Solution Projects

- **API**: Controllers and configuration of the ASP.NET Core application.
- **Service**: Business logic and application services.
- **Infrastructure**: Infrastructure implementation, including database access.
- **Domain**: Domain models.
- **Tests.Unit**: Unit tests.
