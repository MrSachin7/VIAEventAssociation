using Microsoft.Extensions.DependencyInjection;
using VIAEvent.Infrastructure.SqliteDmPersistence;
using VIAEventAssociation.Infrastructure.EfcQueries;

namespace IntegrationTests.EndpointTests.Abstractions;

public abstract class BaseEndpointTest : IClassFixture<EndpointTestsWebAppFactory>, IAsyncDisposable {

    private readonly IServiceScope _scope;

    protected HttpClient Client { get; }

    protected SqliteWriteDbContext WriteDbContext { get; }

    protected VeadatabaseProductionContext ReadDbContext { get; }

    protected BaseEndpointTest(EndpointTestsWebAppFactory factory) {
        _scope = factory.Services.CreateScope();
        WriteDbContext = _scope.ServiceProvider.GetRequiredService<SqliteWriteDbContext>();
        ReadDbContext = _scope.ServiceProvider.GetRequiredService<VeadatabaseProductionContext>();

        WriteDbContext.Database.EnsureDeleted();
        ReadDbContext.Database.EnsureDeleted();


        WriteDbContext.Database.EnsureCreated();
        ReadDbContext.Database.EnsureCreated();

        Client = factory.CreateClient();
    }

    public async ValueTask DisposeAsync() {
        if (_scope is IAsyncDisposable scopeAsyncDisposable)
            await scopeAsyncDisposable.DisposeAsync();
        else
            _scope.Dispose();
    }
}