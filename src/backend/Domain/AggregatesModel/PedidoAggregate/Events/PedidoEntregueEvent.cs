using EnterpriseFlow.Domain.SeedWork;

namespace EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;

public class PedidoEntregueEvent : IDomainEvent
{
    public PedidoEntregueEvent(Pedido pedido) => Pedido = pedido;

    public Pedido Pedido { get; }
}
