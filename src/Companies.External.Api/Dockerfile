FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build
COPY src/Companies.External.Api/Companies.External.Api.csproj src/Companies.External.Api/
COPY src/Companies.Protos/Companies.Protos.csproj src/Companies.Protos/
RUN dotnet restore "src/Companies.External.Api/Companies.External.Api.csproj"
COPY . .
WORKDIR "/build/src/Companies.External.Api"
RUN dotnet build "Companies.External.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Companies.External.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Companies.External.Api.dll"]