using MediatR;

namespace EnterpriseFlow.Domain.SeedWork;

/// <summary>
/// Marca um evento de domínio disparado por uma entidade e publicado via mediator.
/// </summary>
public interface IDomainEvent : INotification
{
}
