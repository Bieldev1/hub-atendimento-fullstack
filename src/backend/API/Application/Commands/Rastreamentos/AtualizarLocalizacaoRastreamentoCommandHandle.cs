using EnterpriseFlow.API.Application.Mappings;
using EnterpriseFlow.API.Application.Models;
using EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;
using EnterpriseFlow.Domain.Common.Results;
using EnterpriseFlow.Domain.Services;
using MediatR;

namespace EnterpriseFlow.API.Application.Commands.Rastreamentos;

public class AtualizarLocalizacaoRastreamentoCommandHandle
    : IRequestHandler<AtualizarLocalizacaoRastreamentoCommand, Result<HistoricoRastreamentoDto>>
{
    private static readonly TimeSpan DuracaoLock = TimeSpan.FromSeconds(5);

    private readonly IRastreamentoRepository rastreamentoRepository;
    private readonly ICacheService cacheService;

    public AtualizarLocalizacaoRastreamentoCommandHandle(
        IRastreamentoRepository rastreamentoRepository,
        ICacheService cacheService)
    {
        this.rastreamentoRepository = rastreamentoRepository;
        this.cacheService = cacheService;
    }

    public async Task<Result<HistoricoRastreamentoDto>> Handle(
        AtualizarLocalizacaoRastreamentoCommand request,
        CancellationToken cancellationToken)
    {
        // Evita condição de corrida quando múltiplos eventos de localização do mesmo pedido
        // chegam concorrentemente (ex.: dois callbacks do provedor de geolocalização).
        var chaveLock = $"rastreamento:pedido:{request.PedidoId}:lock";
        var tokenPosse = await cacheService.TentarAdquirirLockAsync(chaveLock, DuracaoLock);

        if (tokenPosse is null)
            return Result<HistoricoRastreamentoDto>.Fail(
                ResultCode.BusinessError,
                "Já existe uma atualização de localização em andamento para este pedido.");

        try
        {
            var rastreamento = await rastreamentoRepository.ObterPorPedidoIdAsync(request.PedidoId);

            if (rastreamento is null)
                return Result<HistoricoRastreamentoDto>.Fail(
                    ResultCode.BadRequest,
                    $"Nenhum rastreamento encontrado para o pedido {request.PedidoId}.");

            Result resultAtualizar = rastreamento.AtualizarLocalizacao(new Localizacao(request.Latitude, request.Longitude));

            if (!resultAtualizar.Valid)
                return new Result<HistoricoRastreamentoDto>().SetFromAnother(resultAtualizar);

            await rastreamentoRepository.AtualizarAsync(rastreamento);

            return Result<HistoricoRastreamentoDto>.Ok(rastreamento.ParaDto());
        }
        finally
        {
            await cacheService.LiberarLockAsync(chaveLock, tokenPosse);
        }
    }
}
