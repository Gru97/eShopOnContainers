FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/services/catalog/Catalog.API/Catalog.API.csproj src/services/catalog/Catalog.API/
COPY src/BuildingBlocks/IntegrationEventLogEF/IntegrationEventLogEF.csproj src/BuildingBlocks/IntegrationEventLogEF/
COPY src/BuildingBlocks/EventBus/EventBus/EventBus.csproj src/BuildingBlocks/EventBus/EventBus/
COPY src/BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj src/BuildingBlocks/EventBus/EventBusRabbitMQ/
RUN dotnet restore src/services/catalog/Catalog.API/Catalog.API.csproj
COPY . .
WORKDIR /src/src/services/catalog/Catalog.API
RUN dotnet build Catalog.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Catalog.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
