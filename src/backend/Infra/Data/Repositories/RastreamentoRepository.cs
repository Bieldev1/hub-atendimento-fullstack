using EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;
using EnterpriseFlow.Infra.Data.Options;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EnterpriseFlow.Infra.Data.Repositories;

/// <summary>
/// Repositório do agregado Rastreamento (MongoDB). Diferente do <see cref="BaseRepository{TDbContext,TAggregateRoot}"/>
/// (SQL Server), não existe ChangeTracker/UnitOfWork compartilhado aqui — cada operação já persiste
/// e dispara os Domain Events coletados logo em seguida, dentro do próprio método.
/// </summary>
public class RastreamentoRepository : IRastreamentoRepository
{
    private readonly IMongoCollection<Rastreamento> collection;
    private readonly IMediator mediator;

    public RastreamentoRepository(MongoContext context, IOptions<MongoOptions> mongoOptions, IMediator mediator)
    {
        collection = context.Database.GetCollection<Rastreamento>(mongoOptions.Value.RastreamentoCollectionName);
        this.mediator = mediator;
    }

    public async Task<Rastreamento?> ObterPorIdAsync(string id) =>
        await collection.Find(it => it.Id == id).FirstOrDefaultAsync();

    public async Task<Rastreamento?> ObterPorPedidoIdAsync(long pedidoId) =>
        await collection.Find(it => it.PedidoId == pedidoId).FirstOrDefaultAsync();

    public async Task AdicionarAsync(Rastreamento rastreamento)
    {
        await collection.InsertOneAsync(rastreamento);
        await DispatchDomainEventsAsync(rastreamento);
    }

    public async Task AtualizarAsync(Rastreamento rastreamento)
    {
        await collection.ReplaceOneAsync(it => it.Id == rastreamento.Id, rastreamento);
        await DispatchDomainEventsAsync(rastreamento);
    }

    private async Task DispatchDomainEventsAsync(Rastreamento rastreamento)
    {
        var domainEvents = rastreamento.DomainEvents.ToList();
        rastreamento.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
