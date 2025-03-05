var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/companies/{cnpj}", (string cnpj) => new { Cnpj = cnpj, Name = "Company Test", });

app.Run();
