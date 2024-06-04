using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VIAEvent.Infrastructure.SqliteDmPersistence;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<SqliteWriteDbContext> {

    public SqliteWriteDbContext CreateDbContext(string[] args) {
        var optionsBuilder = new DbContextOptionsBuilder<SqliteWriteDbContext>();
        optionsBuilder.UseSqlite(@"Data Source = VEADatabaseProduction.db");
        return new SqliteWriteDbContext(optionsBuilder.Options);    }
}