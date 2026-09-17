using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EnterpriseFlow.Infra.Data;

public class EnterpriseFlowDbContext : DbContext
{
    public EnterpriseFlowDbContext(DbContextOptions<EnterpriseFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pedido> Pedido { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
