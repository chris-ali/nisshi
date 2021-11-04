# Stage 1 - Build Container
FROM mcr.microsoft.com/dotnet/sdk:5.0.402-alpine3.13-amd64 as build

WORKDIR /build
COPY . .

# Need to install dotnet-format and npm first
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-format
RUN apk add --update npm

RUN dotnet run -p build/build.csproj

# Stage 2 - Runtime Container
FROM mcr.microsoft.com/dotnet/aspnet:5.0.11-alpine3.13-amd64

RUN apk add --update tzdata npm

COPY --from=build /build/publish /app
RUN npm install

COPY --from=build /build/wait-for /app/wait-for
RUN chmod +x /app/wait-for

WORKDIR /app
EXPOSE 5000

#ENTRYPOINT [ "dotnet", "Nisshi.dll" ]
