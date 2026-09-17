using EnterpriseFlow.Domain.Repositories;

namespace EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;

/// <summary>
/// Repositório do agregado Pedido. Persistência relacional (SQL Server via EF Core).
/// </summary>
public interface IPedidoRepository : IRepository<Pedido>
{
}
