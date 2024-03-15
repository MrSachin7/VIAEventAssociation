using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Event;

public class EventDescriptionTests {
     
    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenValidDescription_WhenCreatingEventDescription_ThenReturnsSuccessResult(string description) {
        // Arrange
        // Act
        Result<EventDescription> result = EventDescription.Create(description);
        Assert.True(result.IsSuccess);
        Assert.True(result.Payload!.Value.Equals(description));
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventDescriptions), MemberType = typeof(EventFactory))]
    public void GivenInValidDescription_WhenCreatingEventDescription_ThenReturnsFailureResult_WithCorrectError(string description) {
        // Arrange
        // Act
        Result<EventDescription> result = EventDescription.Create(description);
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.DescriptionMustBeLessThan250Chars, result.Error!.Messages);
    }

}