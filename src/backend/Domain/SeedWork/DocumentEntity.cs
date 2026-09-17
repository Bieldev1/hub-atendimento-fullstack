namespace EnterpriseFlow.Domain.SeedWork;

/// <summary>
/// Classe base para agregados persistidos como documento (ex.: MongoDB). Assim como
/// <see cref="Entity"/>, não declara Id: o agregado concreto declara o próprio Id (string).
/// </summary>
public abstract class DocumentEntity : IEntity
{
    private readonly List<IDomainEvent> domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent eventItem) => domainEvents.Add(eventItem);

    public void RemoveDomainEvent(IDomainEvent eventItem) => domainEvents.Remove(eventItem);

    public void ClearDomainEvents() => domainEvents.Clear();
}
