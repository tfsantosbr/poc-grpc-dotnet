using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration; ;

builder.Services.AddHttpClient("CompaniesExternalApi", client =>
{
    var baseAddress = configuration["CompaniesExternalApi:BaseAddress"]
        ?? throw new InvalidOperationException("Base address for CompaniesExternalApi is not configured.");

    client.BaseAddress = new Uri(baseAddress);
});

var app = builder.Build();

app.MapGet("/companies/{cnpj}", async (string cnpj, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("CompaniesExternalApi");

    var response = await client.GetAsync($"/companies/{cnpj}");

    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var company = JsonSerializer.Deserialize<object>(content);

    return Results.Json(company);
});

app.Run();
