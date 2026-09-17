using EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate.Events;
using EnterpriseFlow.Domain.Common.Results;
using EnterpriseFlow.Domain.SeedWork;

namespace EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;

/// <summary>
/// Agregado raiz do rastreamento logístico de um pedido: posição atual do entregador
/// e histórico bruto de rotas. Persistência como documento (MongoDB).
/// </summary>
public class Rastreamento : DocumentEntity, IAggregateRoot
{
    // Não é readonly de propósito: o driver do MongoDB desserializa reatribuindo o campo
    // (Expression.Assign não suporta campo readonly/initonly), diferente do EF Core, que só
    // adiciona itens na coleção já existente.
    private List<PosicaoHistorico> historico = new();

    protected Rastreamento() { }

    private Rastreamento(long pedidoId, long entregadorId, Localizacao localizacaoInicial)
    {
        Id = Guid.NewGuid().ToString("N");
        PedidoId = pedidoId;
        EntregadorId = entregadorId;
        LocalizacaoAtual = localizacaoInicial;
        Status = StatusRastreamento.EmTransito;

        historico.Add(new PosicaoHistorico(localizacaoInicial, DateTime.UtcNow));
    }

    public string Id { get; private set; } = string.Empty;

    public long PedidoId { get; private set; }

    public long EntregadorId { get; private set; }

    public Localizacao LocalizacaoAtual { get; private set; } = null!;

    public StatusRastreamento Status { get; private set; }

    public IReadOnlyCollection<PosicaoHistorico> Historico => historico.AsReadOnly();

    /// <summary>
    /// Inicia o rastreamento de um pedido despachado, a partir da localização inicial do entregador.
    /// </summary>
    public static Rastreamento Iniciar(long pedidoId, long entregadorId, Localizacao localizacaoInicial) =>
        new(pedidoId, entregadorId, localizacaoInicial);

    /// <summary>
    /// Registra uma nova posição do entregador, atualizando a localização atual e o histórico,
    /// e dispara <see cref="RastreamentoLocalizacaoAtualizadaEvent"/>.
    /// </summary>
    public Result AtualizarLocalizacao(Localizacao novaLocalizacao)
    {
        if (Status == StatusRastreamento.Entregue)
            return Result.Fail(
                ResultCode.BusinessError,
                "Não é possível atualizar a localização de um rastreamento já entregue.");

        LocalizacaoAtual = novaLocalizacao;
        historico.Add(new PosicaoHistorico(novaLocalizacao, DateTime.UtcNow));

        AddDomainEvent(new RastreamentoLocalizacaoAtualizadaEvent(this));

        return Result.Ok();
    }

    /// <summary>
    /// Finaliza o rastreamento quando o pedido é entregue.
    /// </summary>
    public void FinalizarEntrega()
    {
        Status = StatusRastreamento.Entregue;
    }
}
