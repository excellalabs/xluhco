FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./xluhco.web/xluhco.web.csproj ./xluhco.web
COPY ./xluhco.web.tests/xluhco.web.tests.csproj ./xluhco.web.tests
COPY ./xluhco.sln ./
RUN dotnet restore

# Run Tests
RUN dotnet test

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "aspnetcoreapp.dll"]