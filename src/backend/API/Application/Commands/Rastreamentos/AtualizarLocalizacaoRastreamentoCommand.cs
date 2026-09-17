using EnterpriseFlow.API.Application.Models;
using EnterpriseFlow.Domain.Common.Results;
using MediatR;

namespace EnterpriseFlow.API.Application.Commands.Rastreamentos;

public class AtualizarLocalizacaoRastreamentoCommand : IRequest<Result<HistoricoRastreamentoDto>>
{
    public AtualizarLocalizacaoRastreamentoCommand(long pedidoId, double latitude, double longitude)
    {
        PedidoId = pedidoId;
        Latitude = latitude;
        Longitude = longitude;
    }

    public long PedidoId { get; }

    public double Latitude { get; }

    public double Longitude { get; }
}
