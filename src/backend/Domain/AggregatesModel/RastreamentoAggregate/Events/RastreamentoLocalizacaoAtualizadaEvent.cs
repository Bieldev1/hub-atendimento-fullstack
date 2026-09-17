using EnterpriseFlow.Domain.SeedWork;

namespace EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate.Events;

public class RastreamentoLocalizacaoAtualizadaEvent : IDomainEvent
{
    public RastreamentoLocalizacaoAtualizadaEvent(Rastreamento rastreamento) => Rastreamento = rastreamento;

    public Rastreamento Rastreamento { get; }
}
