using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;

namespace VIAEvent.Infrastructure.SqliteDmPersistence;

public class SqliteWriteDbContext : DbContext {

    public SqliteWriteDbContext(DbContextOptions<SqliteWriteDbContext> options) : base(options) { }

    public DbSet<VeaEvent> VeaEvents => Set<VeaEvent>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Location> Locations => Set<Location>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteWriteDbContext).Assembly);
    }
}