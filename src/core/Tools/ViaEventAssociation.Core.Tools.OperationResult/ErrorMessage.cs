namespace ViaEventAssociation.Core.Tools.OperationResult;

public class ErrorMessage : Enumeration {
    public static readonly ErrorMessage TitleMustBeBetween3And75Chars =
        new(0, "Event title must be between 3 and 75 characters");

    public static readonly ErrorMessage DescriptionMustBeLessThan250Chars =
        new(0, "Event Description must be less than 250 characters");

    public static readonly ErrorMessage StartTimeMustBeBeforeEndTime =
        new(0, "Event start time cannot be after event end time");

    public static readonly ErrorMessage EventDurationMustBeMoreThan1Hour =
        new(0, "Event duration cannot be less than 1 hour");

    public static readonly ErrorMessage EventDurationMustBeLessThan10Hour =
        new(0, "Event duration cannot be less than 1 hour");

    public static readonly ErrorMessage EventCannotSpanBetween1AmAnd8Am =
        new(0, "Event cannot take place from 1am to 8am");

    public static readonly ErrorMessage EventStartTimeCannotBeInPast =
        new(0, "Event start time must not be on past");

    public static readonly ErrorMessage EventCannotStartBefore8Am =
        new(0, "Event start time cannot be before 8 am");

    public static readonly ErrorMessage MaxGuestsNotLessThan5 =
        new(0, "Maximum number of Guests cannot be less than 5");

    public static readonly ErrorMessage MaxGuestsNotMoreThan50 =
        new(0, "Maximum number of Guests cannot be more than 50");

    public static readonly ErrorMessage ActiveEventIsUnmodifiable =
        new(0, "Active event cannot be modified");

    public static readonly ErrorMessage ActiveEventCannotBePrivate =
        new(0, "Active event cannot be made private");

    public static readonly ErrorMessage ActiveEventCannotReduceMaxGuests =
        new(0, "Maximum number of guests cannot be reduced in an active event");

    public static readonly ErrorMessage TitleMustBeSetBeforeMakingAnEventReady =
        new(0, "Title must be set before making an event ready");

    public static readonly ErrorMessage DescriptionMustBeSetBeforeMakingAnEventReady =
        new(0, "Description must be set before making an event ready");

    public static readonly ErrorMessage EventDurationMustBeSetBeforeMakingAnEventReady =
        new(0, "Event duration must be set before making an event ready");

    public static readonly ErrorMessage EventInThePastCannotBeReady =
        new(0, "Event with start time at past cannot be made ready");

    public static readonly ErrorMessage ActiveEventCannotBeMadeReady =
        new(0, "Active event cannot be made ready");

    public static readonly ErrorMessage CancelledEventIsUnmodifiable =
        new(0, "Cancelled event cannot be modified");

    public static readonly ErrorMessage CancelledEventCannotBeMadeReady =
        new(0, "Cancelled event cannot be made ready");

    public static readonly ErrorMessage CancelledEventCannotBeActivated =
        new(0, "Cancelled event cannot be activated");


    public static readonly ErrorMessage UnParsableGuid =
        new(0, "The provided guid value is not parsable");


    private ErrorMessage() {
    }

    private ErrorMessage(int value, string displayName) : base(value, displayName) {
    }
}