using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

/*
 Todo
 * I will not include this class anywhere because i am still not sure on how this
 should be used. I will ask Troels about this.
 */
public class LocationAvailabilityInterval : ValueObject {
    internal DateTime StartTime { get; set; }
    internal DateTime EndTime { get; set; }


    private LocationAvailabilityInterval(DateTime startTime, DateTime endTime) {
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Result<LocationAvailabilityInterval> Create(DateTime startTime, DateTime endTime,
        ISystemTime systemTime) {
        return Result.ToBuilder(ErrorCode.BadRequest)
            .AssertWithError(
                () => StartDateBeforeEndDate(startTime, endTime),
                ErrorMessage.StartTimeMustBeBeforeEndTime
            )
            .AssertWithError(
                () => StartDateNotInPast(startTime, systemTime),
                ErrorMessage.StartTimeCannotBeInPast
            )
            .AssertWithError(
                () => EventDurationAtLeastOneHour(startTime, endTime),
                ErrorMessage.DurationMustBeMoreThan1Hour
            )
            .WithPayload(new LocationAvailabilityInterval(startTime, endTime))
            .Build();
    }

    private static bool StartDateBeforeEndDate(DateTime startDateTime, DateTime endDateTime) {
        return startDateTime < endDateTime;
    }

    private static bool EventDurationAtLeastOneHour(DateTime startDateTime, DateTime endDateTime) {
        return endDateTime - startDateTime >= TimeSpan.FromHours(1);
    }

    private static bool StartDateNotInPast(DateTime startDateTime, ISystemTime systemTime) {
        return startDateTime > systemTime.CurrentTime();
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return StartTime;
        yield return EndTime;
    }
}