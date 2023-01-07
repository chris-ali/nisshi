# Nisshi, Japanese for Log

A simple logbook application for pilots to log flights and for maintenance or repairs to be recorded per vehicle. Provides for a unified location for simulated and real flights to be logged. 

![NisshiLogbook](https://user-images.githubusercontent.com/15899769/211160948-18d481dd-336a-4e0f-8d5d-720e4d351f2d.png)

![NisshiAircraft](https://user-images.githubusercontent.com/15899769/211160998-3957a353-eba9-479f-8c38-94ce0e6f79cc.png)

Calculates detailed flight analytics broken down by aircraft type, conditions of flight, etc, which are displayed on the dashboard of the application. 

![NisshiDashboard](https://user-images.githubusercontent.com/15899769/211161010-52b4b803-056d-4628-8b2d-209cb6f1c89f.png)

Configuration supports display of user-desired columns in logbook as well as dark theme,

Designed using:
- [Angular 13](https://github.com/angular/angular) 
- [ASP .NET 6.0](https://github.com/dotnet/aspnetcore)

Implements:
- WebAPI
- Swagger endpoints
- CQRS pattern via [MediatR](https://github.com/jbogard/MediatR)
- JWT authentication 
- Target dependency graph via [Bullseye](https://github.com/adamralph/bullseye)
- EFCore 5 interacting with MariaDB/MySQL
- Docker containerization
- xUnit integration tests
- i18n support for English, German and Japanese
- [dotnet-format](https://github.com/dotnet/format) tool to enforce style
- [OData](https://github.com/OData/AspNetCoreOData) for some queries

## Development Environment

Along with the .NET 6.0 SDK, this project requires npm to be installed to install Angular assets.

If no environment variables are defined, an in-memory EFCore database is used with sample data seeded on application startup.

If an instance of MySQL or MariaDB is desired for the database:
 - Run the db-dump SQL script in `./Database` to instantiate the nisshi databse and create a user to connect with
 - Set the environment variable `ASPNETCORE_Nisshi_DatabaseProvider` to `mysql`
 - Set the environment variable `ASPNETCORE_Nisshi_ConnectionString` to `server=localhost;uid=nisshiuser;pwd=saishoNoYuuza1?;database=nisshi` (modify the port/host as needed for your machine)

The project can be run locally from the dotnet CLI, using `dotnet run -p ./src/Nisshi/Nisshi.csproj` from the project root folder. Then open a web browser and navigate to `https://localhost:5001/`. Swagger endpoints are accessible by appending `swagger` to that URL.

Any changes to the Angular project in `./src/Nisshi/ClientApp` will automatically reload the page.

## Build and Publish

[![Build and Test](https://github.com/chris-ali/nisshi/actions/workflows/buildAndTest.yml/badge.svg)](https://github.com/chris-ali/nisshi/actions/workflows/buildAndTest.yml)

This project has full Docker support and can be build and deployed by simply running `docker-compose up -d` from the project root directory. 

If Docker is not desired, you can run `dotnet run -p build/build.csproj` from the project root directory to build the project, which will output the published artifacts in `./publish`.

PRs and pushes/merges to `master` will trigger the GitHub build pipeline which runs a project-wide format check, and then builds and tests the project.
