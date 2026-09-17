using EnterpriseFlow.Domain.SeedWork;

namespace EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;

public class PedidoCriadoEvent : IDomainEvent
{
    public PedidoCriadoEvent(Pedido pedido) => Pedido = pedido;

    public Pedido Pedido { get; }
}
