FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build
COPY src/Companies.Api/Companies.Api.csproj src/Companies.Api/
COPY src/Companies.Protos/Companies.Protos.csproj src/Companies.Protos/
RUN dotnet restore "src/Companies.Api/Companies.Api.csproj"
COPY . .
WORKDIR "/build/src/Companies.Api"
RUN dotnet build "Companies.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Companies.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Companies.Api.dll"]