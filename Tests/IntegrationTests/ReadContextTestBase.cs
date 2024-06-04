using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Infrastructure.EfcQueries;
using Xunit.Abstractions;

namespace IntegrationTests;

public abstract class ReadContextTestBase {

    protected VeadatabaseProductionContext SetupReadContext() {
        DbContextOptionsBuilder<VeadatabaseProductionContext> optionsBuilder = new();
        string testDataBaseName = "Test " + Guid.NewGuid() + " db";
        optionsBuilder.UseSqlite(@"Data Source = " + testDataBaseName);

        var context = new VeadatabaseProductionContext(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }

    protected void PrintPayload<T>(T payload, ITestOutputHelper testOutputHelper) {
        testOutputHelper.WriteLine(JsonSerializer.Serialize(payload, new JsonSerializerOptions {WriteIndented = true}));
    }

    
}