using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate.Events;
using EnterpriseFlow.Domain.Services;
using MediatR;

namespace EnterpriseFlow.API.Application.EventHandlers;

/// <summary>
/// Além de publicar no barramento de eventos, este é o ponto de extensão natural para acionar
/// a fila de integração com transportadoras (RabbitMQ) e iniciar o rastreamento do pedido.
/// </summary>
public class PedidoDespachadoEventHandler : INotificationHandler<PedidoDespachadoEvent>
{
    private const string TopicoCicloDeVida = "pedidos.ciclo-de-vida";
    private const string FilaTransportadoras = "logistica.transportadoras.despacho";

    private readonly IMessagePublisher messagePublisher;

    public PedidoDespachadoEventHandler(IMessagePublisher messagePublisher) =>
        this.messagePublisher = messagePublisher;

    public async Task Handle(PedidoDespachadoEvent notification, CancellationToken cancellationToken)
    {
        await messagePublisher.PublicarAsync(TopicoCicloDeVida, new
        {
            Evento = nameof(PedidoDespachadoEvent),
            notification.Pedido.Id,
            Status = notification.Pedido.Status.ToString()
        }, cancellationToken);

        await messagePublisher.PublicarAsync(FilaTransportadoras, new
        {
            notification.Pedido.Id,
            notification.Pedido.ClienteId
        }, cancellationToken);
    }
}
