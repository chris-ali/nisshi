# Stage 1 - Build Container
FROM mcr.microsoft.com/dotnet/sdk:6.0.102-alpine3.14-amd64 as build

WORKDIR /build
COPY . .

# Need to install dotnet-format and npm first
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN apk add --update npm

RUN dotnet run --project build/build.csproj

# Stage 2 - Runtime Container
FROM mcr.microsoft.com/dotnet/aspnet:6.0.2-alpine3.14-amd64

RUN apk add --update tzdata npm

COPY --from=build /build/publish /app
RUN npm install

# wait-for waits for db to be online before starting webapp
COPY --from=build /build/resources/wait-for /app/wait-for
RUN chmod +x /app/wait-for

WORKDIR /app
EXPOSE 5000
