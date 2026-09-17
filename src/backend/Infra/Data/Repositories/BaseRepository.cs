using EnterpriseFlow.Domain.Repositories;
using EnterpriseFlow.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnterpriseFlow.Infra.Data.Repositories;

/// <summary>
/// Implementação base de repositório para EF Core, compartilhada pelos repositórios concretos.
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TAggregateRoot"></typeparam>
public abstract class BaseRepository<TDbContext, TAggregateRoot> : IRepository<TAggregateRoot>
    where TDbContext : DbContext
    where TAggregateRoot : class, IAggregateRoot
{
    protected readonly TDbContext context;
    private readonly DbSet<TAggregateRoot> entities;

    protected BaseRepository(TDbContext context)
    {
        this.context = context;
        entities = context.Set<TAggregateRoot>();
    }

    public virtual async Task<TAggregateRoot?> GetByIdAsync(object id, params Expression<Func<TAggregateRoot, IEnumerable<object>>>[] relatedEntitiesToLoad)
    {
        TAggregateRoot? obj = await entities.FindAsync(id);

        if (obj is null)
            return null;

        foreach (var relatedEntityToLoad in relatedEntitiesToLoad)
            await context.Entry(obj).Collection(relatedEntityToLoad).LoadAsync();

        return obj;
    }

    public virtual void Add(TAggregateRoot entity) => entities.Add(entity);

    public virtual void Update(TAggregateRoot entity) => context.Update(entity);

    public virtual void Delete(TAggregateRoot entity) => entities.Remove(entity);
}
