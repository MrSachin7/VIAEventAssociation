using UnitTests.Common.Stubs;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Factories;

public static class EventFactory {
    public static VeaEvent GetDraftEvent() {
        return VeaEvent.Empty(new TestSystemTime());
    }

    public static VeaEvent GetReadyEvent() {
        VeaEvent veaEvent = GetDraftEvent();
        veaEvent.UpdateDescription(GetValidEventDescription());
        veaEvent.UpdateTitle(GetValidEventTitle());
        veaEvent.UpdateEventDuration(GetValidEventDuration());
        veaEvent.MakeReady();

        // Assert that the event is ready before returning
        Assert.Equal(EventStatus.Ready, veaEvent.Status);
        return veaEvent;
    }

    public static VeaEvent GetActiveEvent() {
        VeaEvent veaEvent = GetReadyEvent();
        veaEvent.MakeActive();
        // Assert that the event is active before returning
        Assert.Equal(EventStatus.Active, veaEvent.Status);
        return veaEvent;
    }

    public static VeaEvent GetCancelledEvent() {
        VeaEvent veaEvent = GetActiveEvent();
        veaEvent.MakeCancelled();

        // Assert that the event is cancelled before returning
        Assert.Equal(EventStatus.Cancelled, veaEvent.Status);
        return veaEvent;
    }

    public static EventTitle GetValidEventTitle() {
        return EventTitle.From("Valid Title").Payload!;
    }


    public static IEnumerable<object[]> GetValidEventTitles() {
        return new List<object[]> {
            new object[] {"3le"},
            // A string with 75 characters
            new object[] {"a".PadRight(75, 'a')},

            // A string with 75 characters special characters
            new object[] {"a".PadRight(75, '#')},
            // A string with 30 characters
            new object[] {"a".PadRight(30, 'a')},
            new object[] {GetValidEventTitle().Value}
        };
    }

    public static IEnumerable<object[]> GetInValidEventTitles() {
        return new List<object[]> {
            // Empty string
            new object[] {""},

            new object[] {"2l"},
            // A string with 76 characters
            new object[] {"a".PadRight(76, 'a')},
            // A string with 75 ++ characters
            new object[] {"a".PadRight(200, 'a')}  ,
            // new object[] {null},

        };
    }

    public static EventDescription GetValidEventDescription() {
        return EventDescription.From("Valid Description").Payload!;
    }

    public static IEnumerable<object[]> GetValidEventDescriptions() {
        return new List<object[]> {
            new object[] {""},
            // A string with 75 characters
            new object[] {"a".PadRight(75, 'a')},
            new object[] {"a".PadRight(100, '#')},
            new object[] {"a".PadRight(250, 'a')},
            new object[] {GetValidEventDescription().Value}
        };
    }

    public static IEnumerable<object[]> GetInValidEventDescriptions() {
        return new List<object[]> {
            // Empty string
            new object[] {"a".PadRight(251, 'a')},
            // A string with 75 ++ characters
            new object[] {"a".PadRight(300, 'a')},
            // new object[] {null},

        };
    }

    public static EventDuration GetValidEventDuration() {
        ISystemTime systemTime = new TestSystemTime();
        DateTime validStartTime = new DateTime(systemTime.CurrentTime().Year, systemTime.CurrentTime().Month,
            systemTime.CurrentTime().AddDays(1).Day, 8, 0, 0);
        DateTime validEndTime = new DateTime(systemTime.CurrentTime().Year, systemTime.CurrentTime().Month,
            systemTime.CurrentTime().AddDays(1).Day, 12, 0, 0);
        return EventDuration.From(validStartTime, validEndTime, systemTime).Payload!;
    }

    // Any method using this data should be using the TestSystemTime
    public static IEnumerable<object[]> GetValidEventDurations() {
        ISystemTime systemTime = new TestSystemTime();
        DateTime currentTime = systemTime.CurrentTime();

        // Duration 1: 1-hour span on the next day
        DateTime start1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 12, 0, 0);
        DateTime end1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 13, 0, 0);

        // Duration 2: One hour difference span on 2 days
        DateTime start2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 23, 0, 0);
        DateTime end2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(2).Day, 0, 0, 0);

        // Duration 3: 10-hour difference between start and end time span on 2 days
        DateTime start3 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 15, 0, 0);
        DateTime end3 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(2).Day, 1, 0, 0);

        // Duration 4: 10-hour difference between start and end time 
        DateTime start4 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 13, 0, 0);
        DateTime end4 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 23, 0, 0);

        // Duration 5: 5-hour difference between start and end time 
        DateTime start5 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 13, 0, 0);
        DateTime end5 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 18, 0, 0);


        return new List<object[]> {
            new object[] {GetValidEventDuration().StartDateTime, GetValidEventDuration().EndDateTime},
            new object[] {start1, end1},
            new object[] {start2, end2},
            new object[] {start3, end3},
            new object[] {start4, end4},
            new object[] {start5, end5},
            // new object[] {null, null},
        };
    }

    public static IEnumerable<object[]> GetInValidEventDurations() {
        ISystemTime systemTime = new TestSystemTime();
        DateTime currentTime = systemTime.CurrentTime();

        // Duration 1: Start date before end date
        DateTime start1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 12, 0, 0);
        DateTime end1 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 11, 59, 59);

        // Duration 2: Start date in past
        DateTime start2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(-1).Day, 23, 0, 0);
        DateTime end2 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(2).Day, 0, 0, 0);

        // Duration 3: Events Duration less than 1 hour
        DateTime start3 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 15, 0, 0);
        DateTime end3 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 15, 59, 59);

        // Duration 4: Events duration more than 10 hours 
        DateTime start4 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 8, 0, 0);
        DateTime end4 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 18, 0, 1);

        // Duration 5: Start ime before 8am 
        DateTime start5 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 4, 0, 0);
        DateTime end5 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 9, 0, 0);

        // Duration 6: Events spans between 1am and 8am
        DateTime start6 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 1, 0, 0);
        DateTime end6 = new DateTime(currentTime.Year, currentTime.Month, currentTime.AddDays(1).Day, 8, 0, 0);

        return new List<object[]> {
            new object[] {start1, end1},
            new object[] {start2, end2},
            new object[] {start3, end3},
            new object[] {start4, end4},
            new object[] {start5, end5},
            new object[] {start6, end6}
        };
    }


    public static IEnumerable<object[]> GetValidEventMaxGuests() {
        return new List<object[]>() {
            new object[] {5},
            new object[] {50},
            new object[] {10},
            new object[] {20},
            new object[] {30},
            new object[] {40}
        };
    }

    public static IEnumerable<object[]> GetInValidEventMaxGuests() {
        return new List<object[]>() {
            new object[] {0},
            new object[] {2},
            new object[] {4},
            new object[] {51},
            new object[] {100},
            new object[] {1500} ,
            // new object[] {null}
        };
    }


}