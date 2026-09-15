using EnterpriseFlow.Domain.SeedWork;
using System.Linq.Expressions;

namespace EnterpriseFlow.Domain.Repositories;

/// <summary>
/// Repositório base para raízes de agregado.
/// </summary>
/// <typeparam name="TAggregateRoot"></typeparam>
public interface IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
{
    Task<TAggregateRoot?> GetByIdAsync(object id, params Expression<Func<TAggregateRoot, IEnumerable<object>>>[] relatedEntitiesToLoad);

    void Add(TAggregateRoot entity);

    void Update(TAggregateRoot entity);

    void Delete(TAggregateRoot entity);
}
