# Stage 1 - Build Container
FROM mcr.microsoft.com/dotnet/sdk:5.0.402-focal as build

WORKDIR /build
COPY . .
RUN dotnet run -p build/build.csproj

#Need to install NodeJS first
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs
#RUN npm install

# Stage 2 - Runtime Container
FROM mcr.microsoft.com/dotnet/runtime:5.0.11-focal

#RUN apk add --no-cache tzdata
COPY --from=build /build/publish /app
WORKDIR /app
EXPOSE 5000

ENTRYPOINT [ "dotnet", "Nisshi.dll" ]