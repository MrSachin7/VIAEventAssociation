﻿using Moq;
using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using VIAEventAssociation.Core.Domain.Services;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Services;

public class UpdateEventMaxGuestsServiceTests {
    [Fact]
    public async Task GivenEventInMaxGuestUpdatableCondition_WhenLocationHasEnoughMaxGuests_ThenReturnsSuccessResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        Location location = LocationFactory.GetValidLocation();

        // Arrange that the location has 30 max guests
        const int locationMaxGuests = 30;
        LocationMaxGuests newLocationMaxGuests = LocationMaxGuests.Create(locationMaxGuests).Payload!;
        location.UpdateLocationMaxGuests(newLocationMaxGuests);
        veaEvent.UpdateLocation(location);
        Assert.Equal(locationMaxGuests, location.LocationMaxGuests.Value);
        Assert.Equal(veaEvent.LocationId, location.Id);



        Mock<ILocationRepository> locationRepo = new Mock<ILocationRepository>();
        Mock<IEventRepository> eventRepo = new Mock<IEventRepository>();

        locationRepo.Setup(x => x.FindAsync(location.Id)).ReturnsAsync(location);
        UpdateEventMaxGuestsService updateEventMaxGuestsService = new UpdateEventMaxGuestsService(locationRepo.Object, eventRepo.Object);


        // Act
        const int newEventMaxGuests = 10;
        EventMaxGuests newEventMaxGuestsObj = EventMaxGuests.Create(newEventMaxGuests).Payload!;

        Result result =  await updateEventMaxGuestsService.Handle(veaEvent.Id, newEventMaxGuestsObj);

        // Assert
        Assert.True(result.IsSuccess);
        // Assert that the max guests is updated
        Assert.Equal(newEventMaxGuests, veaEvent.MaxGuests.Value);
    }

    [Fact]
    public async Task GivenEventInMaxGuestUpdatableCondition_WhenLocationDoesNotHaveEnoughMaxGuests_ThenReturnsFailureResult() {
        // Arrange
        VeaEvent veaEvent = EventFactory.GetDraftEvent();
        Location location = LocationFactory.GetValidLocation();

        // Arrange that the location has 30 max guests
        const int locationMaxGuests = 30;
        LocationMaxGuests newLocationMaxGuests = LocationMaxGuests.Create(locationMaxGuests).Payload!;
        location.UpdateLocationMaxGuests(newLocationMaxGuests);
        veaEvent.UpdateLocation(location);
        Assert.Equal(locationMaxGuests, location.LocationMaxGuests.Value);
        Assert.Equal(veaEvent.LocationId, location.Id);



        Mock<ILocationRepository> locationRepo = new Mock<ILocationRepository>();
        Mock<IEventRepository> eventRepo = new Mock<IEventRepository>();

        locationRepo.Setup(x => x.FindAsync(location.Id)).ReturnsAsync(location);
        UpdateEventMaxGuestsService updateEventMaxGuestsService = new UpdateEventMaxGuestsService(locationRepo.Object, eventRepo.Object);


        // Act with max guests more than location allows
        const int newEventMaxGuests = 35;
        EventMaxGuests newEventMaxGuestsObj = EventMaxGuests.Create(newEventMaxGuests).Payload!;

        Result result =await updateEventMaxGuestsService.Handle(veaEvent.Id, newEventMaxGuestsObj);

        // Assert
        Assert.True(result.IsFailure);
        // Assert that the max guests is updated
        Assert.Contains(ErrorMessage.EventMaxGuestsCannotExceedLocationMaxGuests, result.Error!.Messages);
    }
}