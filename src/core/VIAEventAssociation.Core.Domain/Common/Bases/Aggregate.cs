namespace VIAEventAssociation.Core.Domain.Common.Bases;

public abstract class Aggregate<TId> : Entity<TId> where TId : Id {

    protected Aggregate(TId id) : base(id) {

    }

    // For EFC
    protected Aggregate(){}
    
}