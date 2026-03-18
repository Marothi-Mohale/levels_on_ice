FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NuGet.Config ./
COPY Directory.Build.props ./
COPY LevelsOnIceSalon.sln ./
COPY LevelsOnIceSalon.Domain/LevelsOnIceSalon.Domain.csproj ./LevelsOnIceSalon.Domain/
COPY LevelsOnIceSalon.Infrastructure/LevelsOnIceSalon.Infrastructure.csproj ./LevelsOnIceSalon.Infrastructure/
COPY LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj ./LevelsOnIceSalon.Web/

RUN dotnet restore ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj --configfile ./NuGet.Config

COPY . .

RUN dotnet publish ./LevelsOnIceSalon.Web/LevelsOnIceSalon.Web.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:10000 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LevelsOnIceSalon.Web.dll"]
