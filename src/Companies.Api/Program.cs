using System.Text.Json;
using Companies.Protos;
using Grpc.Net.Client;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; ;

builder.Services.AddHttpClient("CompaniesExternalApi", client =>
{
    var baseAddress = configuration["CompaniesExternalApi:BaseAddress"]
        ?? throw new InvalidOperationException("Base address for CompaniesExternalApi is not configured.");

    client.BaseAddress = new Uri(baseAddress);
});

var app = builder.Build();

app.MapGet("/companies/{cnpj}/rest", async (string cnpj, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("CompaniesExternalApi");

    var response = await client.GetAsync($"/companies/{cnpj}");

    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var company = JsonSerializer.Deserialize<object>(content);

    return Results.Json(company);
});

app.MapGet("/companies/{cnpj}/grpc", async (string cnpj, IHttpClientFactory httpClientFactory) =>
{
    // Configura o canal gRPC
    using var channel = GrpcChannel.ForAddress("http://companies-external-api:8081");
    var client = new CompanyService.CompanyServiceClient(channel);

    var company = await client.GetCompanyByCnpjAsync(
        new GetCompanyByCnpjRequest { Cnpj = cnpj }
    );

    return Results.Json(company);
});

app.Run();
