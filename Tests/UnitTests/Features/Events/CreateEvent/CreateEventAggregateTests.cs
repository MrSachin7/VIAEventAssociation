using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Events.CreateEvent;

public class CreateEventAggregateTests {


    // U1
    [Fact]
    public void CreateEvent_CreatesAnEmptyEvent_WithDefaultValues() {
        // Arrange and act
        VeaEvent veaEvent = VeaEvent.Empty(EventId.New());

        // Assert
        Assert.NotNull(veaEvent);
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);
        Assert.Equal(EventTitle.Default(), veaEvent.Title);
        Assert.Equal(EventDescription.Default(), veaEvent.Description);
        Assert.Equal(EventMaxGuests.Default(), veaEvent.MaxGuests);
        
    }


}