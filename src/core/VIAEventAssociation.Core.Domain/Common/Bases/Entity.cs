namespace VIAEventAssociation.Core.Domain.Common.Bases;

public abstract class Entity<TId> {

    public TId Id { get; init; }

    protected Entity(TId id) {
        Id = id;
    }

    // For EFC serializaiton
    protected Entity() {

    }
    
}