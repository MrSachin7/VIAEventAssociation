using Microsoft.EntityFrameworkCore;
using VIAEvent.Infrastructure.SqliteDmPersistence;
using VIAEvent.Infrastructure.SqliteDmPersistence.Contracts;
using VIAEvent.Infrastructure.SqliteDmPersistence.GuestPersistence;
using VIAEvent.Infrastructure.SqliteDmPersistence.LocationPersistence;
using VIAEvent.Infrastructure.SqliteDmPersistence.UnitOfWork;
using VIAEvent.Infrastructure.SqliteDmPersistence.VeaEventPersistence;
using VIAEventAssociation.Core.AppEntry.Dispatcher;
using VIAEventAssociation.Core.AppEntry.Dispatcher.Decorators;
using VIAEventAssociation.Core.Application;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using VIAEventAssociation.Core.Domain.Contracts;
using VIAEventAssociation.Core.QueryContracts.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;
using VIAEventAssociation.Infrastructure.EfcQueries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<SqliteWriteDbContext>(options => options.UseSqlite(
    @"Data Source = D:\SEM7\DCA\VIAEventAssociation\src\Infrastructure\VIAEvent.Infrastructure.SqliteDmPersistence\VEADatabaseProduction.db"));

builder.Services.AddDbContext<VeadatabaseProductionContext>(options => options.UseSqlite(
    @"Data Source = D:\SEM7\DCA\VIAEventAssociation\src\Infrastructure\VIAEvent.Infrastructure.SqliteDmPersistence\VEADatabaseProduction.db"));

builder.Services.AddScoped<ISystemTime, DefaultSystemTime>();
builder.Services.AddScoped<IUnitOfWork, SqliteUnitOfWork>();
builder.Services.AddScoped<IMapper, ObjectMapper>();

// Repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

// Domain contracts
builder.Services.AddScoped<IUniqueEmailChecker, UniqueEmailChecker>();

// Command Handlers
builder.Services.RegisterCommandHandlers();

// Query Handlers
builder.Services.RegisterQueryHandlers();

builder.Services.AddProblemDetails();

// Decorators
builder.Services.AddScoped<CommandDispatcher>();

builder.Services.AddScoped<IDispatcher>(sp =>
{
    var commandDispatcher = sp.GetRequiredService<CommandDispatcher>();
    var unitOfWork = sp.GetRequiredService<IUnitOfWork>();
    var logger = sp.GetRequiredService<ILogger<ExecutionTimerDecorator>>(); // Use ILogger from DI container

    var transactionDecorator = new TransactionDecorator(commandDispatcher, unitOfWork);
    return new ExecutionTimerDecorator(transactionDecorator, logger);
});

builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "VeaEvent Web Api", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();

app.Run();

public partial class Program {

}