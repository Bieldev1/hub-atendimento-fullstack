using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;
using EnterpriseFlow.Domain.Services;
using MediatR;

namespace EnterpriseFlow.API.Application.EventHandlers;

public class PedidoEntregueEventHandler : INotificationHandler<PedidoEntregueEvent>
{
    private const string Topico = "pedidos.ciclo-de-vida";

    private readonly IMessagePublisher messagePublisher;

    public PedidoEntregueEventHandler(IMessagePublisher messagePublisher) =>
        this.messagePublisher = messagePublisher;

    public Task Handle(PedidoEntregueEvent notification, CancellationToken cancellationToken) =>
        messagePublisher.PublicarAsync(Topico, new
        {
            Evento = nameof(PedidoEntregueEvent),
            notification.Pedido.Id,
            Status = notification.Pedido.Status.ToString()
        }, cancellationToken);
}
