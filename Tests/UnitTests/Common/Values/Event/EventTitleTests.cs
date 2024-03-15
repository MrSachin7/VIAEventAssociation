using UnitTests.Common.Factories;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Values.Event;

public class EventTitleTests {
    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenValidTitle_WhenCreatingEventTitle_ThenReturnsSuccessResult(string title) {
        // Arrange
        // and
        // Act
        Result<EventTitle> result = EventTitle.Create(title);
        Assert.True(result.IsSuccess);
        Assert.True(result.Payload!.Value.Equals(title));
    }

    [Theory]
    [MemberData(nameof(EventFactory.GetInValidEventTitles), MemberType = typeof(EventFactory))]
    public void GivenInValidTitle_WhenCreatingEventTitle_ThenReturnsFailureResult(string title) {
        // Arrange
        // and
        // Act
        Result<EventTitle> result = EventTitle.Create(title);
        Assert.True(result.IsFailure);
        Assert.Contains(ErrorMessage.TitleMustBeBetween3And75Chars, result.Error!.Messages);
    }
}