using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

internal class EventDuration : ValueObject {
    internal DateTime StartDateTime { get; private init; }
    internal DateTime EndDateTime { get; private init; }

    private EventDuration() {
    }


    public static Result<EventDuration> From(DateTime startDateTime, DateTime endDateTime) {
        return Result<EventDuration>.AsBuilder(ErrorCode.BadRequest, new EventDuration() {
                StartDateTime = startDateTime,
                EndDateTime = endDateTime
            })
            .AssertWithError(
                () => StartDateBeforeEndDate(startDateTime, endDateTime),
                ErrorMessage.StartTimeMustBeBeforeEndTime
            )
            .AssertWithError(
                () => StartDateNotInPast(startDateTime),
                ErrorMessage.EventStartTimeCannotBeInPast
            )
            .AssertWithError(
                () => EventDurationMoreThanOneHour(startDateTime, endDateTime),
                ErrorMessage.EventDurationMustBeMoreThan1Hour
            )
            .AssertWithError(
                () => EventDurationLessThanTenHour(startDateTime, endDateTime),
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

    private static bool StartDateNotInPast(DateTime startDateTime) {
        return startDateTime > DateTime.Now;
    }

    private static bool EventDurationMoreThanOneHour(DateTime startDateTime, DateTime endDateTime) {
        return endDateTime - startDateTime > TimeSpan.FromHours(1);
    }

    private static bool EventDurationLessThanTenHour(DateTime startDateTime, DateTime endDateTime) {
        return endDateTime - startDateTime < TimeSpan.FromHours(10);
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