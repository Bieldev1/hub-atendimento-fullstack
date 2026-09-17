using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;
using EnterpriseFlow.Domain.Services;
using MediatR;

namespace EnterpriseFlow.API.Application.EventHandlers;

public class PedidoPagamentoAprovadoEventHandler : INotificationHandler<PedidoPagamentoAprovadoEvent>
{
    private const string Topico = "pedidos.ciclo-de-vida";

    private readonly IMessagePublisher messagePublisher;

    public PedidoPagamentoAprovadoEventHandler(IMessagePublisher messagePublisher) =>
        this.messagePublisher = messagePublisher;

    public Task Handle(PedidoPagamentoAprovadoEvent notification, CancellationToken cancellationToken) =>
        messagePublisher.PublicarAsync(Topico, new
        {
            Evento = nameof(PedidoPagamentoAprovadoEvent),
            notification.Pedido.Id,
            Status = notification.Pedido.Status.ToString()
        }, cancellationToken);
}
