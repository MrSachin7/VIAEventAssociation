using System.Net.Http.Json;
using IntegrationTests.EndpointTests.Abstractions;
using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

namespace IntegrationTests.EndpointTests.Events;

public class CreateEventTests : BaseEndpointTest {

    public CreateEventTests(EndpointTestsWebAppFactory factory ) : base(factory) {
    }

    [Fact]
    public async Task CreateEvent_ReturnsOK() {
        // Arrange and act
        var response = await Client.PostAsync("/api/events", null);
        CreateEventResponse? createEventResponse = await response.Content.ReadFromJsonAsync<CreateEventResponse>();

        Assert.NotNull(createEventResponse);
        string id = createEventResponse.Id;


        // Verifies that the event was created
        var a =await ReadDbContext.VeaEvents.ToListAsync();

        // For some wierd reason, sqlite stores the id in uppercase
        bool exists =await ReadDbContext.VeaEvents.AnyAsync(@event => @event.Id.ToLower().Equals(id));
        Assert.True(exists);
    }
}