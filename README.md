# LTC Web Gateway

## 1) What this project is

`ltt-ltc-web-gateway` is the reverse-proxy entry point for backend services.

It uses YARP-style reverse proxy configuration to route calls to downstream APIs such as:

- Administration API
- Customer API

Main project:

- `LTC.WebGateWay`

## 2) Prerequisites

- .NET SDK 10+
- Downstream APIs running (administration/customer) on configured URLs

## 3) Configure and run

Routes and destinations are configured in:

- `LTC.WebGateWay/appsettings.Development.json`

Run gateway:

```bash
dotnet run --project LTC.WebGateWay/LTC.WebGateWay.csproj
```

Swagger aggregation endpoint is exposed by the gateway at:

- `/ltc/swagger`

## 4) Route to code: Client -> Gateway -> API -> DB

1. Client calls gateway route (for example `/ltc/customer-service/*`).
2. Gateway matches `ReverseProxy:Routes`.
3. Gateway forwards to `ReverseProxy:Clusters` destination.
4. Downstream API handles business logic and talks to its own DB.

Relevant code/config:

- `LTC.WebGateWay/Program.cs`
- `LTC.WebGateWay/appsettings.Development.json`

## 5) How to add a new API through gateway

1. Add a new route in `ReverseProxy:Routes`.
2. Add a matching destination in `ReverseProxy:Clusters`.
3. Add Swagger mapping entry in destination `Swaggers` section (if you want gateway swagger aggregation).
4. Restart gateway.

Example route pattern:

- `/ltc/new-service/{**catch-all}` -> `https://localhost:<service-port>`

## 6) Notes

- Gateway itself does not own application business tables.
- Data persistence is in downstream services, not in this project.
