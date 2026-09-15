namespace EnterpriseFlow.Domain.SeedWork;

/// <summary>
/// Abstrai a persistência das alterações pendentes em uma unidade de trabalho.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
