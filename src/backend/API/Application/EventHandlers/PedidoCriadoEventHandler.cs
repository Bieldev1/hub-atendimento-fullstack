using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;
using EnterpriseFlow.Domain.Services;
using MediatR;

namespace EnterpriseFlow.API.Application.EventHandlers;

/// <summary>
/// Publica o evento de ciclo de vida do pedido no tópico de streaming (Kafka), permitindo que
/// outros contextos (logística, faturamento, notificações) reajam de forma assíncrona.
/// </summary>
public class PedidoCriadoEventHandler : INotificationHandler<PedidoCriadoEvent>
{
    private const string Topico = "pedidos.ciclo-de-vida";

    private readonly IMessagePublisher messagePublisher;

    public PedidoCriadoEventHandler(IMessagePublisher messagePublisher) =>
        this.messagePublisher = messagePublisher;

    public Task Handle(PedidoCriadoEvent notification, CancellationToken cancellationToken) =>
        messagePublisher.PublicarAsync(Topico, new
        {
            Evento = nameof(PedidoCriadoEvent),
            notification.Pedido.Id,
            notification.Pedido.ClienteId,
            Status = notification.Pedido.Status.ToString(),
            notification.Pedido.ValorTotal
        }, cancellationToken);
}
