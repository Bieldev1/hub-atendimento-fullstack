namespace EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;

/// <summary>
/// Registro imutável de uma posição ocupada pelo entregador em um instante do trajeto.
/// </summary>
public class PosicaoHistorico
{
    protected PosicaoHistorico() { }

    public PosicaoHistorico(Localizacao localizacao, DateTime dataHoraUtc)
    {
        Localizacao = localizacao;
        DataHoraUtc = dataHoraUtc;
    }

    public Localizacao Localizacao { get; private set; } = null!;

    public DateTime DataHoraUtc { get; private set; }
}
