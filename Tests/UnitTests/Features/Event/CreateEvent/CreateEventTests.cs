using VIAEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Features.Event.CreateEvent;

public class CreateEventTests {

    [Fact]
    public void CreateEvent_CreatesAnEmptyEvent_WithDefaultValues() {
        // Arrange and act
        VeaEvent veaEvent = VeaEvent.Empty();

        // Assert
        Assert.NotNull(veaEvent);
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
        Assert.Equal(EventVisibility.Private, veaEvent.Visibility);
        Assert.Equal(EventTitle.Default(), veaEvent.Title);
        Assert.Equal(EventDescription.Default(), veaEvent.Description);
        Assert.Equal(EventMaxGuests.Default(), veaEvent.MaxGuests);
        
    }


}