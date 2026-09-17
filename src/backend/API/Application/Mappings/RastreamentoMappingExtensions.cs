using EnterpriseFlow.API.Application.Models;
using EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;

namespace EnterpriseFlow.API.Application.Mappings;

public static class RastreamentoMappingExtensions
{
    public static HistoricoRastreamentoDto ParaDto(this Rastreamento rastreamento) => new()
    {
        Id = rastreamento.Id,
        PedidoId = rastreamento.PedidoId,
        EntregadorId = rastreamento.EntregadorId,
        Status = rastreamento.Status.ToString(),
        LocalizacaoAtual = new PosicaoHistoricoDto
        {
            Latitude = rastreamento.LocalizacaoAtual.Latitude,
            Longitude = rastreamento.LocalizacaoAtual.Longitude,
            DataHoraUtc = rastreamento.Historico.LastOrDefault()?.DataHoraUtc ?? default
        },
        Historico = rastreamento.Historico.Select(posicao => new PosicaoHistoricoDto
        {
            Latitude = posicao.Localizacao.Latitude,
            Longitude = posicao.Localizacao.Longitude,
            DataHoraUtc = posicao.DataHoraUtc
        }).ToList()
    };
}
