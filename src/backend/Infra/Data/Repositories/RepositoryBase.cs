using EnterpriseFlow.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnterpriseFlow.Infra.Data.Repositories;

/// <summary>
/// Implementação base de repositório para EF Core, compartilhada pelos repositórios concretos.
/// </summary>
/// <typeparam name="TAggregateRoot"></typeparam>
public abstract class RepositoryBase<TAggregateRoot> where TAggregateRoot : Entity, IAggregateRoot
{
    protected readonly ApplicationDbContext Context;

    protected RepositoryBase(ApplicationDbContext context) => Context = context;

    public IUnitOfWork UnitOfWork => Context;

    public virtual async Task<TAggregateRoot?> GetByIdAsync(object id, params Expression<Func<TAggregateRoot, IEnumerable<object>>>[] relatedEntitiesToLoad)
    {
        IQueryable<TAggregateRoot> query = Context.Set<TAggregateRoot>();

        foreach (var include in relatedEntitiesToLoad)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(e => Equals(EF.Property<object>(e, "Id"), id));
    }

    public virtual void Add(TAggregateRoot entity) => Context.Set<TAggregateRoot>().Add(entity);

    public virtual void Update(TAggregateRoot entity) => Context.Entry(entity).State = EntityState.Modified;

    public virtual void Delete(TAggregateRoot entity) => Context.Set<TAggregateRoot>().Remove(entity);
}
