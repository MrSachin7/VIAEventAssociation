using UnitTests.Common.Factories;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Events.UpdateTitle;

public class UpdateEventTitleCommandTests {
    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenValidEventIdAndTitle_UpdateTitleCommand_CanBeCreated(string validTitle) {
        // Arrange and act
        Result<UpdateEventTitleCommand> result =
            UpdateEventTitleCommand.Create(EventFactory.ValidEventId, validTitle);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenValidEventIdAndInvalidTitle_UpdateTitleCommand_CannotBeCreated(string inValidTitle) {
        // Arrange and act
        Result<UpdateEventTitleCommand>
            result = UpdateEventTitleCommand.Create(EventFactory.ValidEventId, inValidTitle);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.TitleMustBeBetween3And75Chars, result.Error!.Messages);
    }


    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenInvalidEventIdAndValidTitle_UpdateTitleCommand_CannotBeCreated(string validTitle) {
        // Arrange and act
        Result<UpdateEventTitleCommand> result =
            UpdateEventTitleCommand.Create("", validTitle);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.UnParsableGuid, result.Error!.Messages);
    }
}