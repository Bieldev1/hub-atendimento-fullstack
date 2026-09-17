using EnterpriseFlow.Domain.Common.Results;
using EnterpriseFlow.Domain.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EnterpriseFlow.Infra.Data;

/// <summary>
/// Implementação de <see cref="IUnitOfWork"/> que dispara os Domain Events coletados pelo
/// ChangeTracker ANTES de fazer o SaveChanges — assim os handlers rodam na mesma unidade de
/// trabalho/transação das entidades que os originaram.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly EnterpriseFlowDbContext context;
    private readonly IMediator mediator;

    public UnitOfWork(EnterpriseFlowDbContext context, IMediator mediator)
    {
        this.context = context;
        this.mediator = mediator;
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Result resultDispatch = await DispatchDomainEventsAsync(cancellationToken);

        if (!resultDispatch.Valid)
            return resultDispatch;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (DbUpdateException exception)
        {
            return Result.Fail(ResultCode.GenericError, $"Falha ao persistir as alterações: {exception.Message}");
        }
    }

    private async Task<Result> DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        List<EntityEntry<IEntity>> domainEntities = context.ChangeTracker
            .Entries<IEntity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .ToList();

        List<IDomainEvent> domainEvents = domainEntities
            .SelectMany(entry => entry.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entry => entry.Entity.ClearDomainEvents());

        try
        {
            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent, cancellationToken);

            return Result.Ok();
        }
        catch (Exception exception)
        {
            return Result.Fail(ResultCode.GenericError, $"Falha ao publicar eventos de domínio: {exception.Message}");
        }
    }
}
