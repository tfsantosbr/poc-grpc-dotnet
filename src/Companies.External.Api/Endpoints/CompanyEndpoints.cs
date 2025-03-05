
namespace Companies.External.Api.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this WebApplication app)
    {
        app.MapGet("/companies/{cnpj}", GetCompanyByCnpj).WithName("Get Company By CNPJ");
    }

    private static IResult GetCompanyByCnpj(string cnpj)
        => Results.Ok(new { Name = "Company Test", Cnpj = cnpj });
}
