using UnitTests.Common.Factories;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateMaxGuests;

public class UpdateEventMaxGuestsCommandTests {

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventMaxGuests), MemberType = typeof(EventFactory))]
    public void GivenValidEventIdAndMaxGuests_UpdateMaxGuestsCommand_CanBeCreated(int validMaxGuests) {
        // Arrange and act
        Result<UpdateEventMaxGuestsCommand> result =
            UpdateEventMaxGuestsCommand.Create(EventFactory.ValidEventId, validMaxGuests);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventMaxGuests), MemberType = typeof(EventFactory))]
    public void GivenInvalidEventIdAndValidMaxGuests_UpdateMaxGuestsCommand_CannotBeCreated(int validMaxGuests) {
        // Arrange and act
        Result<UpdateEventMaxGuestsCommand> result =
            UpdateEventMaxGuestsCommand.Create("", validMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventMaxGuests), MemberType = typeof(EventFactory))]
    public void GivenValidEventIdAndInvalidMaxGuests_UpdateMaxGuestsCommand_CannotBeCreated(int invalidMaxGuests) {
        // Arrange and act
        Result<UpdateEventMaxGuestsCommand> result =
            UpdateEventMaxGuestsCommand.Create(EventFactory.ValidEventId, invalidMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
    }

    
}