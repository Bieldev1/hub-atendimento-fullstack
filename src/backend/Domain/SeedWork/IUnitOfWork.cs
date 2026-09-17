using EnterpriseFlow.Domain.Common.Results;

namespace EnterpriseFlow.Domain.SeedWork;

/// <summary>
/// Abstrai a persistência das alterações pendentes em uma unidade de trabalho.
/// Retorna <see cref="Result"/> (em vez do int cru do EF Core) para que falhas de
/// persistência (ex.: violação de constraint) cheguem ao handler como dado, não exceção.
/// </summary>
public interface IUnitOfWork
{
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}
