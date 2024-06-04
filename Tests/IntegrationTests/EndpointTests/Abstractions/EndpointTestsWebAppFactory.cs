using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VIAEvent.Infrastructure.SqliteDmPersistence;
using VIAEventAssociation.Infrastructure.EfcQueries;

namespace IntegrationTests.EndpointTests.Abstractions;

public abstract class EndpointTestsWebAppFactory : WebApplicationFactory<Program> {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureTestServices(services => {
            services.RemoveAll(typeof(DbContextOptions<SqliteWriteDbContext>));
            string testDataBaseName = "Test " + Guid.NewGuid() + " db";
            services.AddDbContext<SqliteWriteDbContext>(
                options => options.UseSqlite
                    (@"Data Source = " + testDataBaseName)
            );

            services.RemoveAll(typeof(DbContextOptions<VeadatabaseProductionContext>));
            services.AddDbContext<VeadatabaseProductionContext>(
                options => options.UseSqlite
                    (@"Data Source = " + testDataBaseName)
            );
        });
    }
}