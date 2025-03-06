using System.Text.Json;
using Companies.Protos;
using static Companies.Protos.CompanyService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHttpClient("CompaniesExternalApi", client =>
{
    var baseAddress = configuration["CompaniesExternalApi:BaseAddress"]
        ?? throw new InvalidOperationException("Base address for 'CompaniesExternalApi' is not configured.");

    client.BaseAddress = new Uri(baseAddress);
});

builder.Services.AddGrpcClient<CompanyServiceClient>(client =>
{
    var baseAddress = configuration["CompaniesExternalGrpc:BaseAddress"]
        ?? throw new InvalidOperationException("Base address for 'CompaniesExternalGrpc' is not configured.");

    client.Address = new Uri(baseAddress);
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

app.MapGet("/companies/{cnpj}/grpc", async (string cnpj, CompanyServiceClient companyServiceClient) =>
{
    var company = await companyServiceClient.GetCompanyByCnpjAsync(
        new GetCompanyByCnpjRequest { Cnpj = cnpj }
    );

    return Results.Json(company);
});

app.Run();
