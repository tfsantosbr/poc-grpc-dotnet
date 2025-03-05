using Companies.External.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços gRPC ao contêiner
builder.Services.AddGrpc();

var app = builder.Build();

// Configura o endpoint REST
app.MapGet("/companies/{cnpj}", (string cnpj) => new { Cnpj = cnpj, Name = "Company Test", });

// Configura o endpoint gRPC
app.MapGrpcService<CompanyGrpcService>();

app.Run();
