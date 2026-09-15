# syntax=docker/dockerfile:1

# ---- Build ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NuGet.Config .
COPY API/EnterpriseFlow.API.csproj API/
COPY Domain/EnterpriseFlow.Domain.csproj Domain/
COPY Infra/EnterpriseFlow.Infra.csproj Infra/
RUN dotnet restore API/EnterpriseFlow.API.csproj

COPY API/ API/
COPY Domain/ Domain/
COPY Infra/ Infra/
RUN dotnet publish API/EnterpriseFlow.API.csproj -c Release -o /app/publish --no-restore

# ---- Runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnterpriseFlow.API.dll"]
