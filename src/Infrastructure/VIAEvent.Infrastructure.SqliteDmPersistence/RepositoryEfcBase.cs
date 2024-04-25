using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Domain.Common.Repository;

namespace VIAEvent.Infrastructure.SqliteDmPersistence;

public abstract class RepositoryEfcBase<TAgg, TId> : IRepository<TAgg, TId>
    where TAgg : Aggregate<TId>
    where TId : Id {

    private readonly DbContext _context;

    protected RepositoryEfcBase(DbContext context) {
        _context = context;
    }

    public virtual Task<TAgg?> FindAsync(TId id) {
        return _context.Set<TAgg>().FindAsync(id).AsTask();
    }

    public virtual Task AddAsync(TAgg aggregate) {
        return _context.Set<TAgg>().AddAsync(aggregate).AsTask();
    }

    public virtual void Remove(TAgg aggregate) {
        _context.Set<TAgg>().Remove(aggregate);
    }

}