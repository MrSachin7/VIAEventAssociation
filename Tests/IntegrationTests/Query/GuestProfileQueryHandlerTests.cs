using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;
using VIAEventAssociation.Infrastructure.EfcQueries;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;
using VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;
using Xunit.Abstractions;

namespace IntegrationTests.Query;

public class GuestProfileQueryHandlerTests : ReadContextTestBase{
    private readonly ITestOutputHelper _testOutputHelper;

    public GuestProfileQueryHandlerTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task HandleAsync_WhenGuestExists_ReturnsSuccessResult() {
        // Arrange
        VeadatabaseProductionContext context = await SetupReadContext().SeedAllDataAsync();
        const string guestId = "230c1a99-d5c7-4fbc-9f48-07ccbb100936";
        GuestProfileQuery.Query query = new GuestProfileQuery.Query(guestId);
        GuestProfileQueryHandler handler = new GuestProfileQueryHandler(context,new QueryTestSystemTime());

        // Act
        Result<GuestProfileQuery.Answer> result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guestId, result.Payload!.GuestProfile.GuestId);

        //Print
        PrintPayload(result.Payload!, _testOutputHelper);

    }


    [Fact]
    public async Task HandleAsync_WhenGuestDoesntExist_ReturnsNotFound() {
        // Arrange
        // No data seeding
        VeadatabaseProductionContext context = SetupReadContext();
        const string guestId = "230c1a99-d5c7-4fbc-9f48-07ccbb100936";
        GuestProfileQuery.Query query = new GuestProfileQuery.Query(guestId);
        GuestProfileQueryHandler handler = new GuestProfileQueryHandler(context,new QueryTestSystemTime());

        // Act
        Result<GuestProfileQuery.Answer> result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.GuestNotFound(guestId), result.Error!.Messages);
    }


    
}