# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json Directory.Build.props Directory.Packages.props ./
COPY src/Warehouse.Domain/Warehouse.Domain.csproj src/Warehouse.Domain/
COPY src/Warehouse.Application/Warehouse.Application.csproj src/Warehouse.Application/
COPY src/Warehouse.Infrastructure/Warehouse.Infrastructure.csproj src/Warehouse.Infrastructure/
COPY src/Warehouse.Api/Warehouse.Api.csproj src/Warehouse.Api/
RUN dotnet restore src/Warehouse.Api/Warehouse.Api.csproj
COPY src/ src/
RUN dotnet publish src/Warehouse.Api/Warehouse.Api.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "Warehouse.Api.dll"]
