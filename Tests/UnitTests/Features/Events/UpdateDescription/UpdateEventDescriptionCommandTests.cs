using UnitTests.Common.Factories;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateDescription;

public class UpdateEventDescriptionCommandTests {
    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenValidEventIdAndDescription_UpdateDescriptionCommand_CanBeCreated(string validDescription) {
        // Arrange and act
        Result<UpdateEventDescriptionCommand> result =
            UpdateEventDescriptionCommand.Create(EventFactory.ValidEventId, validDescription);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenInvalidEventIdAndValidDescription_UpdateDescriptionCommand_CannotBeCreated(string validDescription) {
        // Arrange and act
        Result<UpdateEventDescriptionCommand> result =
            UpdateEventDescriptionCommand.Create("", validDescription);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenValidEventIdAndInvalidDescription_UpdateDescriptionCommand_CannotBeCreated(string invalidDescription) {
        // Arrange and act
        Result<UpdateEventDescriptionCommand> result =
            UpdateEventDescriptionCommand.Create(EventFactory.ValidEventId, invalidDescription);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.DescriptionMustBeLessThan250Chars, result.Error!.Messages);
    }
}