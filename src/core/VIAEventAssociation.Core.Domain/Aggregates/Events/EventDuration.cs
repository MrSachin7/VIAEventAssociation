using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventDuration : ValueObject {
    internal DateTime StartDateTime { get; private init; }
    internal DateTime EndDateTime { get; private init; }

    private EventDuration(DateTime startDateTime, DateTime endDateTime) {
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }


    public static Result<EventDuration> From(DateTime startDateTime, DateTime endDateTime, ISystemTime systemTime) {
        return Result<EventDuration>.AsBuilder(ErrorCode.BadRequest, new EventDuration(startDateTime, endDateTime))
            .AssertWithError(
                () => StartDateBeforeEndDate(startDateTime, endDateTime),
                ErrorMessage.StartTimeMustBeBeforeEndTime
            )
            .AssertWithError(
                () => StartDateNotInPast(startDateTime, systemTime),
                ErrorMessage.EventStartTimeCannotBeInPast
            )
            .AssertWithError(
                () => EventDurationAtLeastOneHour(startDateTime, endDateTime),
                ErrorMessage.EventDurationMustBeMoreThan1Hour
            )
            .AssertWithError(
                () => EventDurationAtMaxTenHour(startDateTime, endDateTime),
                ErrorMessage.EventDurationMustBeLessThan10Hour
            )
            .AssertWithError(
                () => EventCannotSpanBetween1AmAnd8Am(startDateTime, endDateTime),
                ErrorMessage.EventCannotSpanBetween1AmAnd8Am
            )
            .AssertWithError(
                () => EventCannotStartBefore8Am(startDateTime),
                ErrorMessage.EventCannotStartBefore8Am
            )
            .Build();
    }

    private static bool StartDateBeforeEndDate(DateTime startDateTime, DateTime endDateTime) {
        return startDateTime < endDateTime;
    }

    // TODO : ask troels : Should i take ISystemTime here as a parameter , if yes then than would mean I take it as a param on the static factory method too.
    private static bool StartDateNotInPast(DateTime startDateTime, ISystemTime systemTime) {
        return startDateTime > systemTime.CurrentTime();
    }

    private static bool EventDurationAtLeastOneHour(DateTime startDateTime, DateTime endDateTime) {
        return endDateTime - startDateTime >= TimeSpan.FromHours(1);
    }

    private static bool EventDurationAtMaxTenHour(DateTime startDateTime, DateTime endDateTime) {
        return endDateTime - startDateTime <= TimeSpan.FromHours(10);
    }

    private static bool EventCannotSpanBetween1AmAnd8Am(DateTime startDateTime, DateTime endDateTime) {
        TimeSpan eventStartTime = startDateTime.TimeOfDay;
        TimeSpan eventEndTime = endDateTime.TimeOfDay;

        // Check if the event spans between 1 AM and 8 AM
        return !(eventEndTime > TimeSpan.FromHours(1) && eventStartTime < TimeSpan.FromHours(8));
    }        

    private static bool EventCannotStartBefore8Am(DateTime startDateTime) {
        return startDateTime.TimeOfDay >= TimeSpan.FromHours(8);
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return StartDateTime;
        yield return EndDateTime;
    }
}