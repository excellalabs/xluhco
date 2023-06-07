FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./xluhco.web/*.csproj ./xluhco.web/
COPY ./xluhco.web.tests/*.csproj ./xluhco.web.tests/
COPY ./xluhco.sln ./
RUN dotnet restore

# Copy everything else and build
COPY ./xluhco.web ./xluhco.web/
COPY ./xluhco.web.tests ./xluhco.web.tests/
RUN dotnet test xluhco.web.tests
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "xluhco.web.dll"]