FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5200

ENV ASPNETCORE_URLS=http://+:5200
ENV ASPNETCORE_ENVIRONMENT=Development

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY API-Login/API-Login.csproj API-Login/
COPY API-Login/appsettings.Development.json API-Login/
RUN dotnet restore "API-Login/API-Login.csproj"
COPY . .
WORKDIR "/src/API-Login"
RUN dotnet build "API-Login.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "API-Login.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY API-Login/appsettings.Development.json .
ENTRYPOINT ["dotnet", "API-Login.dll"]