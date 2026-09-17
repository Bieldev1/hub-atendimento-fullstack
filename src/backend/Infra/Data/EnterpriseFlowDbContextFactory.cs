using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EnterpriseFlow.Infra.Data;

/// <summary>
/// Fábrica usada pelas ferramentas de design-time do EF Core (ex.: `dotnet ef migrations add`),
/// que não sobem o host completo da API (e, portanto, não resolvem os serviços de Infra ainda
/// não implementados, como cache/mensageria).
/// </summary>
public class EnterpriseFlowDbContextFactory : IDesignTimeDbContextFactory<EnterpriseFlowDbContext>
{
    public EnterpriseFlowDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EnterpriseFlowDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=EnterpriseFlow;Trusted_Connection=True;TrustServerCertificate=True;");

        return new EnterpriseFlowDbContext(optionsBuilder.Options);
    }
}
