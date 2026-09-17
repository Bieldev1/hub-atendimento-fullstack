using EnterpriseFlow.API.Application.Models;
using EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate.Events;
using EnterpriseFlow.Domain.Services;
using MediatR;

namespace EnterpriseFlow.API.Application.EventHandlers;

/// <summary>
/// Atualiza o cache de geolocalização em tempo real (Redis) e publica o evento de streaming (Kafka)
/// para consumidores interessados em rastreamento ao vivo (ex.: dashboard do cliente).
/// </summary>
public class RastreamentoLocalizacaoAtualizadaEventHandler : INotificationHandler<RastreamentoLocalizacaoAtualizadaEvent>
{
    private const string Topico = "rastreamento.localizacao-atualizada";
    private static readonly TimeSpan ExpiracaoCache = TimeSpan.FromMinutes(10);

    private readonly ICacheService cacheService;
    private readonly IMessagePublisher messagePublisher;

    public RastreamentoLocalizacaoAtualizadaEventHandler(ICacheService cacheService, IMessagePublisher messagePublisher)
    {
        this.cacheService = cacheService;
        this.messagePublisher = messagePublisher;
    }

    public async Task Handle(RastreamentoLocalizacaoAtualizadaEvent notification, CancellationToken cancellationToken)
    {
        var rastreamento = notification.Rastreamento;

        var localizacaoAtual = new PosicaoHistoricoDto
        {
            Latitude = rastreamento.LocalizacaoAtual.Latitude,
            Longitude = rastreamento.LocalizacaoAtual.Longitude,
            DataHoraUtc = DateTime.UtcNow
        };

        await cacheService.DefinirAsync(
            $"rastreamento:pedido:{rastreamento.PedidoId}:localizacao-atual",
            localizacaoAtual,
            ExpiracaoCache);

        await messagePublisher.PublicarAsync(Topico, new
        {
            rastreamento.PedidoId,
            rastreamento.EntregadorId,
            localizacaoAtual.Latitude,
            localizacaoAtual.Longitude
        }, cancellationToken);
    }
}
