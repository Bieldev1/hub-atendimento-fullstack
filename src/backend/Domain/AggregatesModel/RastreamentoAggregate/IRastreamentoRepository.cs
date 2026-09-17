namespace EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;

/// <summary>
/// Repositório do agregado Rastreamento. Persistência como documento (MongoDB).
/// Não estende <see cref="EnterpriseFlow.Domain.Repositories.IRepository{TAggregateRoot}"/> porque
/// esse contrato é modelado para persistência relacional via EF Core (Id numérico, Include de
/// navigation properties), o que não se aplica a um repositório de documentos.
/// </summary>
public interface IRastreamentoRepository
{
    Task<Rastreamento?> ObterPorIdAsync(string id);

    Task<Rastreamento?> ObterPorPedidoIdAsync(long pedidoId);

    Task AdicionarAsync(Rastreamento rastreamento);

    Task AtualizarAsync(Rastreamento rastreamento);
}
