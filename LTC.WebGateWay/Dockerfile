FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LTC.WebGateWay/LTC.WebGateWay.csproj", "LTC.WebGateWay/"]
COPY ["LTC.Shared.ServiceDefaults/LTC.Shared.ServiceDefaults.csproj", "LTC.Shared.ServiceDefaults/"]

RUN dotnet restore "./LTC.WebGateWay/LTC.WebGateWay.csproj"
COPY . .
WORKDIR "/src/LTC.WebGateWay"
RUN dotnet publish "./LTC.WebGateWay.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LTC.WebGateWay.dll"]