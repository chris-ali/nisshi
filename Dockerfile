# Stage 1 - Build Container
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-amd64 as build

WORKDIR /build
COPY . .

# Need to install dotnet-format and npm first
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN apk add --update npm

RUN dotnet run --project build/build.csproj

# Stage 2 - Runtime Container
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-amd64

RUN apk add --update tzdata npm

COPY --from=build /build/publish /app

# Need to generate node_modules inside Angular directory
WORKDIR /app/ClientApp
RUN npm install --legacy-peer-deps

# wait-for waits for db to be online before starting webapp
COPY --from=build /build/resources/wait-for /app/wait-for
RUN chmod +x /app/wait-for

WORKDIR /app
EXPOSE 80
EXPOSE 443
