using System.Net;
using System.Text;
using System.Text.Json;
using IntegrationTests.EndpointTests.Abstractions;
using VIAEventAssociation.Core.Domain.Aggregates.Events;

namespace IntegrationTests.EndpointTests.Events;

public class UpdateEventDescription : BaseEndpointTest{

   [Fact]
   public async Task UpdateEventDescription_Returns_NotFoundWhenEventNotFound() {
      string eventId = Guid.NewGuid().ToString();
      var requestBody = new {
         Description = "New description"
      };

      HttpResponseMessage response = await Client.PatchAsync(@$"/api/events/{eventId}/description",
         new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));
      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

   }

   [Fact]
   public async Task UpdateEventDescription_Returns_OkWhenEventFound() {
      // vea event exists
      VeaEvent eventAggregate = VeaEvent.Empty(EventId.New());
      await WriteDbContext.VeaEvents.AddAsync(eventAggregate);
      await WriteDbContext.SaveChangesAsync();

      string eventId = eventAggregate.Id.Value.ToString();
      var requestBody = new {
         Description = "New description"
      };

      HttpResponseMessage response = await Client.PatchAsync(@$"/api/events/{eventId}/description",
         new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));


      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

   }

   public UpdateEventDescription(EndpointTestsWebAppFactory factory) : base(factory) {
   }
}