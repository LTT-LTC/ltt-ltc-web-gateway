FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /workspace

# Copy the entire project
COPY . .

# Restore and publish the host project
WORKDIR /workspace/LTC.WebGateWay
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runner
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "LTC.WebGateWay.dll"]