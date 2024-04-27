using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;
using VIAEventAssociation.Infrastructure.EfcQueries;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;
using VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;
using Xunit.Abstractions;

namespace IntegrationTests.Query;

public class UpcomingEventsQueryHandlerTests : ReadContextTestBase {

    private readonly ITestOutputHelper _testOutputHelper;

    public UpcomingEventsQueryHandlerTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task HandleAsync_WhenEventsExists_ReturnSuccessResult() {
        // Arrange
        VeadatabaseProductionContext context = await SetupReadContext().SeedAllDataAsync();
        const int eventsPerPage = 3;
        UpcomingEventsQuery.Query query = new UpcomingEventsQuery.Query("", 1,eventsPerPage);
        UpcomingEventsQueryHandler handlerTests = new UpcomingEventsQueryHandler(context, new QueryTestSystemTime());

        // Act
        Result<UpcomingEventsQuery.Answer> result = await handlerTests.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);

        // 3 for pagination
        Assert.True(result.Payload!.UpcomingEvents.Count is > 0 and <= eventsPerPage);
        // Print                                                       
        PrintPayload(result.Payload!, _testOutputHelper);
    }


    [Fact]
    public async Task HandleAsync_WhenEventsDoesntExist_ReturnEmptyList() {
        // Arrange
        // No data seeding
        VeadatabaseProductionContext context = SetupReadContext();
        UpcomingEventsQuery.Query query = new UpcomingEventsQuery.Query("", 1, 3);
        UpcomingEventsQueryHandler handler = new UpcomingEventsQueryHandler(context, new QueryTestSystemTime());

        // Act
        Result<UpcomingEventsQuery.Answer> result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Payload!.UpcomingEvents);
    }
}