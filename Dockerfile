FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5500

ENV ASPNETCORE_URLS=http://+:5500
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release

WORKDIR /src
COPY ["OPL_grafana_meilisearch.csproj", "."]
RUN dotnet restore "OPL_grafana_meilisearch.csproj"

COPY . .
RUN dotnet build "OPL_grafana_meilisearch.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release

RUN dotnet publish "OPL_grafana_meilisearch.csproj" -c $configuration -o /app/publish


# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure the correct DLL name
ENTRYPOINT ["dotnet", "OPL_grafana_meilisearch.dll"]