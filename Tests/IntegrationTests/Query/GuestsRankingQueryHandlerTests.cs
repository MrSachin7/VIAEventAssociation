using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;
using VIAEventAssociation.Infrastructure.EfcQueries;
using VIAEventAssociation.Infrastructure.EfcQueries.DataSeeder;
using VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;
using Xunit.Abstractions;

namespace IntegrationTests.Query;

public class GuestsRankingQueryHandlerTests : ReadContextTestBase {

    private readonly ITestOutputHelper _testOutputHelper;

    public GuestsRankingQueryHandlerTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task HandleAsync_WhenGuestsExist_ReturnsSuccessResult() {
        // Arrange
        VeadatabaseProductionContext context = await SetupReadContext().SeedAllDataAsync();
        const int guestsPerPage = 5;
        GuestsRankingQuery.Query query = new GuestsRankingQuery.Query(1, guestsPerPage);
        GuestsRankingQueryHandler handler = new GuestsRankingQueryHandler(context);

        // Act
        Result<GuestsRankingQuery.Answer> result = await handler.HandleAsync(query);



        //Print
        PrintPayload(result.Payload!, _testOutputHelper);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Payload!.Guests);
        Assert.True(result.Payload!.Guests.Count is > 0 and <= guestsPerPage);


    }
}