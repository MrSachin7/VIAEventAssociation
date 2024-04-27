using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;
using VIAEventAssociation.Infrastructure.EfcQueries;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;
using VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;
using Xunit.Abstractions;

namespace IntegrationTests.Query;

public class UnpublishedEventsQueryHandlerTests : ReadContextTestBase {
    private readonly ITestOutputHelper _testOutputHelper;

    public UnpublishedEventsQueryHandlerTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task HandleAsync_WhenEventsExists_ReturnsSuccessResult() {
        // Arrange
        VeadatabaseProductionContext context = await SetupReadContext().SeedAllDataAsync();
        UnpublishedEventsQuery.Query query = new UnpublishedEventsQuery.Query();
        UnpublishedEventsQueryHandler handler = new UnpublishedEventsQueryHandler(context);

        // Act
        Result<UnpublishedEventsQuery.Answer> result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Payload!.DraftsEvents.Count > 0);
        Assert.True(result.Payload!.ReadyEvents.Count > 0);
        Assert.True(result.Payload!.CancelledEvents.Count > 0);

        // Print
        PrintPayload(result.Payload!, _testOutputHelper);
    }

    [Fact]
    public async Task HandleAsync_WhenEventsDoesntExist_ReturnsEmptyLists() {
        // Arrange
        // No data seeding
        VeadatabaseProductionContext context = SetupReadContext();
        UnpublishedEventsQuery.Query query = new UnpublishedEventsQuery.Query();
        UnpublishedEventsQueryHandler handler = new UnpublishedEventsQueryHandler(context);

        // Act
        Result<UnpublishedEventsQuery.Answer> result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Payload!.DraftsEvents);
        Assert.Empty(result.Payload!.ReadyEvents);
        Assert.Empty(result.Payload!.CancelledEvents);
    }
}