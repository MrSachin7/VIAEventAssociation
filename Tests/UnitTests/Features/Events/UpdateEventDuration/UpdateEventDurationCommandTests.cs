using UnitTests.Common.Factories;
using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateEventDuration;

public class UpdateEventDurationCommandTests {
    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDurations), MemberType = typeof(EventFactory))]
    public void GivenValidEventIdAndDuration_UpdateDurationCommand_CanBeCreated(DateTime validStartTime,
        DateTime validEndTime) {
        // Arrange and act
        Result<UpdateEventDurationCommand> result =
            UpdateEventDurationCommand.Create(EventFactory.ValidEventId,
                validStartTime, validEndTime,
                new TestSystemTime());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDurations), MemberType = typeof(EventFactory))]
    public void GivenInvalidEventIdAndValidDuration_UpdateDurationCommand_CannotBeCreated(DateTime validStartTime,
        DateTime validEndTime) {
        // Arrange and act
        Result<UpdateEventDurationCommand> result = UpdateEventDurationCommand.Create("",
            validStartTime, validEndTime,
            new TestSystemTime());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventDurations), MemberType = typeof(EventFactory))]
    public void GivenValidEventIdAndInvalidDuration_UpdateDurationCommand_CannotBeCreated(DateTime invalidStartTime,
        DateTime invalidEndTime) {
        // Arrange and act
        ISystemTime systemTime = new TestSystemTime();
        Result<UpdateEventDurationCommand> result = UpdateEventDurationCommand.Create(EventFactory.ValidEventId,
            invalidStartTime, invalidEndTime, systemTime);

        // Assert
        Assert.True(result.IsFailure);
    }
}