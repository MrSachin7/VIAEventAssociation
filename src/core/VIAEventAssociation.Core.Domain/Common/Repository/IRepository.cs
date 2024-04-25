using VIAEventAssociation.Core.Domain.Common.Bases;

namespace VIAEventAssociation.Core.Domain.Common.Repository;

public interface IRepository<TAgg, in TId> where TId : Id
    where TAgg : Aggregate<TId> {

    // Todo: ask troels if I can return null if not found
    Task<TAgg?> FindAsync(TId id);

    Task AddAsync(TAgg aggregate);

    void Remove(TAgg aggregate);
}