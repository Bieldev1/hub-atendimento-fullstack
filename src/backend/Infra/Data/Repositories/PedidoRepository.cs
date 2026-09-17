using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;

namespace EnterpriseFlow.Infra.Data.Repositories;

public class PedidoRepository : BaseRepository<EnterpriseFlowDbContext, Pedido>, IPedidoRepository
{
    public PedidoRepository(EnterpriseFlowDbContext context) : base(context) { }
}
