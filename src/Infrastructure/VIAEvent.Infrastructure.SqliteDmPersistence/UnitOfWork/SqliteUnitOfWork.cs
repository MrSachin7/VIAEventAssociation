using VIAEventAssociation.Core.Domain.Common.UnitOfWork;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.UnitOfWork;

public class SqliteUnitOfWork : IUnitOfWork{

    private readonly SqliteWriteDbContext _context;

    public SqliteUnitOfWork(SqliteWriteDbContext context) {
        _context = context;
    }

    public Task SaveChangesAsync() {
        return _context.SaveChangesAsync();
    }
}