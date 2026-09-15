namespace EnterpriseFlow.Domain.SeedWork;

/// <summary>
/// Classe base para entidades do domínio.
/// </summary>
public abstract class Entity : IEntity
{
    private readonly List<IDomainEvent> domainEvents = new();

    public int Id { get; protected set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent eventItem) => domainEvents.Add(eventItem);

    public void RemoveDomainEvent(IDomainEvent eventItem) => domainEvents.Remove(eventItem);

    public void ClearDomainEvents() => domainEvents.Clear();

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id != 0 && other.Id != 0 && Id == other.Id;
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity? left, Entity? right) =>
        Equals(left, null) ? Equals(right, null) : left.Equals(right);

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);
}
