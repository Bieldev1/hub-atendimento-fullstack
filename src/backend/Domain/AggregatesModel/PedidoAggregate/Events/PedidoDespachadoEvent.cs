using EnterpriseFlow.Domain.SeedWork;

namespace EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;

public class PedidoDespachadoEvent : IDomainEvent
{
    public PedidoDespachadoEvent(Pedido pedido) => Pedido = pedido;

    public Pedido Pedido { get; }
}
