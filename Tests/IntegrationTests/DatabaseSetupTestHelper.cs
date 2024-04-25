using Microsoft.EntityFrameworkCore;
using VIAEvent.Infrastructure.SqliteDmPersistence;

namespace IntegrationTests;

public abstract class DatabaseSetupTestHelper {
    protected SqliteWriteDbContext SetupContext() {
        DbContextOptionsBuilder<SqliteWriteDbContext> optionsBuilder = new();
        string testDbName = "Test " + Guid.NewGuid() + " db";
        optionsBuilder.UseSqlite(@"Data Source = " + testDbName);

        SqliteWriteDbContext context = new SqliteWriteDbContext(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    protected async Task SaveAndClearAsync<T>(T entity, SqliteWriteDbContext dbContext)
        where T : class {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
    }
}