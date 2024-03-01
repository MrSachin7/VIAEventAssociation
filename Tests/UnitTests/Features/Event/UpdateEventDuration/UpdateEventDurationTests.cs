using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit.Abstractions;

namespace UnitTests.Features.Event.UpdateEventDuration;

public class UpdateEventDurationTests(ITestOutputHelper testOutputHelper) {

    [Theory]
    [MemberData(nameof(EventFactory.GetValidEventDurations), MemberType = typeof(EventFactory))]
    public void GivenValidStartAndEndTime_EventDuration_CanBeCreated(DateTime validStartTime, DateTime validEndTime) {
        ISystemTime systemTime = new TestSystemTime();
        Result<EventDuration> result = EventDuration.From(validStartTime, validEndTime, systemTime);

        if (result.IsFailure) {
            testOutputHelper.WriteLine(result.Error!.ToString());
        }

        Assert.True(result.IsSuccess);
        Assert.Equal(validStartTime, result.Payload!.StartDateTime);
        Assert.Equal(validEndTime, result.Payload!.EndDateTime);
    }
}