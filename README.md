# Nisshi
A simple logbook application for pilots to log flights. Allows for a unified location for simulated and real flights to be logged.

Designed using:
- [Angular 12](https://github.com/angular/angular) 
- [.NET Core 5.0](https://github.com/dotnet/aspnetcore) WebAPI

Implements:
- Swagger endpoints
- CQRS pattern via [MediatR](https://github.com/jbogard/MediatR)
- JWT authentication 
- Target dependency graph via [Bullseye](https://github.com/adamralph/bullseye)
- EFCore 5 interacting with MariaDB/MySQL
- Docker containerization
- xUnit integration tests
- dotnet-format tool to enforce style

[![Build and Test](https://github.com/chris-ali/nisshi/actions/workflows/buildAndTest.yml/badge.svg)](https://github.com/chris-ali/nisshi/actions/workflows/buildAndTest.yml)

## Development Environment

Along with the .NET 5.0 SDK, this project requires npm to be installed to install Angular assets.

If no environment variables are defined, an in-memory EFCore database is used with sample data seeded on application startup.

If an instance of MySQL or MariaDB is desired for the database:
 - Run the db-dump SQL script in `./Database` to instantiate the nisshi databse and a user to connect with
 - Set the environment variable `ASPNETCORE_Nisshi_DatabaseProvider` to `mysql`
 - Set the environment variable `ASPNETCORE_Nisshi_ConnectionString` to `server=localhost;uid=nisshiuser;pwd=saishoNoYuuza1?;database=nisshi` (modify the port/host as needed for your machine)

The project can be run locally from the dotnet CLI, using `dotnet build && dotnet run` from `./src/Nisshi`. Then open a web browser and navigate to `https://localhost:5001/`. Swagger endpoints are accessible by appending `swagger` to that URL.

Any changes to the Angular project in `./src/Nisshi/ClientApp` will automatically reload the page.

## Build and Publish

This project has full Docker support and can be build and deployed by simply running `docker-compose up -d` from the project root directory. 

If Docker is not desired, run `dotnet run -p build/build.csproj` from the project root directory to build the project, which will output the published artifacts in `./publish`.
