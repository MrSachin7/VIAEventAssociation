using VIAEventAssociation.Core.Domain.Common.Bases;

namespace VIAEventAssociation.Core.Domain.Common.Repository;

public interface IRepository<TAgg, in TId> where TId : Id
    where TAgg : Aggregate<TId> {

    Task<TAgg?> FindAsync(TId id);

    Task AddAsync(TAgg aggregate);

    Task DeleteAsync(TAgg aggregate);
}