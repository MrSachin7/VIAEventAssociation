using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;
using VIAEventAssociation.Infrastructure.EfcQueries;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;
using VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;
using Xunit.Abstractions;

namespace IntegrationTests.Query;

public class EventDetailsQueryHandlerTests : ReadContextTestBase{

    private readonly ITestOutputHelper _testOutputHelper;

    public EventDetailsQueryHandlerTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public async Task HandleAsync_WhenEventExists_ReturnsEventDetails() {
        // Arrange
        VeadatabaseProductionContext context =await SetupReadContext().SeedAllDataAsync();
        const int guestsPerPage = 3;
        // const string eventId = "27a5bde5-3900-4c45-9358-3d186ad6b2d7";
        const string eventId = "40ed2fd9-2240-4791-895f-b9da1a1f64e4";


        EventDetailsQuery.Query query = new EventDetailsQuery.Query(eventId, 1, guestsPerPage);
        EventDetailsQueryHandler handler = new EventDetailsQueryHandler(context);
        
        // Act
        Result<EventDetailsQuery.Answer> result = await handler.HandleAsync(query);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(eventId, result.Payload!.VeaEvent.EventId);

        // Print
        PrintPayload(result.Payload!, _testOutputHelper);
      
    }

    [Fact]
    public async Task HandleAsync_WhenEventDoesntExist_ReturnsNotFound() {
        // Arrange
        // No data seeding
        VeadatabaseProductionContext context = SetupReadContext();
        
        const string eventId = "40ed2fd9-2240-4791-895f-b9da1a1f64e4";
        EventDetailsQuery.Query query = new EventDetailsQuery.Query(eventId, 1, 3);
        EventDetailsQueryHandler handler = new EventDetailsQueryHandler(context);
        
        // Act
        Result<EventDetailsQuery.Answer> result = await handler.HandleAsync(query);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.EventNotFound(eventId), result.Error!.Messages);
    }


    
}