using Companies.External.Api.Protos;
using Grpc.Core;

namespace Companies.External.Api.Services;

public class CompanyGrpcService : CompanyService.CompanyServiceBase
{
    public override Task<GetCompanyByCnpjResponse> GetCompanyByCnpj(
        GetCompanyByCnpjRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetCompanyByCnpjResponse
        {
            Cnpj = request.Cnpj,
            Name = "Company Test"
        });
    }
}
