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

Make sure you have Docker and Docker Compose installed on your machine.

1. Clone the repository:

```bash
git clone https://github.com/predodamiao/Product-api
cd Product-api
```

2. Execute o Docker Compose para iniciar a API e o banco de dados:

```bash
docker-compose up -d
```

3. Access the API at http://localhost:80.

## Database Creation Strategy

The database is created using Entity Framework Core's "Code First" approach. This means that the database is automatically generated based on the domain models defined in the project.

## How to Run Migrations

To run the migrations and create the database, run the following commands:

```bash
cd src/API
dotnet ef database update --project .\Infrastructure --startup-project .\Api
```

## Solution Projects

- **API**: Contains the controllers and configuration of the ASP.NET Core application.
- **Service**: Business logic and application services.
- **Infrastructure**: Infrastructure implementation, including database access.
- **Domain**: Domain models and domain logic.
- **Tests.Unit**: Unit tests.
- **Tests.Integration**: Integration tests.
