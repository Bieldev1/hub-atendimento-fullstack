using EnterpriseFlow.Domain.SeedWork;

namespace EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;

public class PedidoPagamentoAprovadoEvent : IDomainEvent
{
    public PedidoPagamentoAprovadoEvent(Pedido pedido) => Pedido = pedido;

    public Pedido Pedido { get; }
}
