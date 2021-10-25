# Stage 1 - Build Container
FROM node:14-alpine3.13 AS node_base
FROM mcr.microsoft.com/dotnet/sdk:5.0.402-alpine3.13-amd64 as build

COPY --from=node_base . .
WORKDIR /build
COPY . .

# Need to install dotnet-format first
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-format

RUN dotnet run -p build/build.csproj

# Stage 2 - Runtime Container
FROM mcr.microsoft.com/dotnet/aspnet:5.0.11-alpine3.13-amd64

RUN apk add --no-cache tzdata
COPY --from=build /build/publish /app
WORKDIR /app
EXPOSE 5000

ENTRYPOINT [ "dotnet", "Nisshi.dll" ]
