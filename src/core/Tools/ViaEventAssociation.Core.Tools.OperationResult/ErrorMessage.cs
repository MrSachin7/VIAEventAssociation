namespace ViaEventAssociation.Core.Tools.OperationResult;

public class ErrorMessage : Enumeration {
    public static readonly ErrorMessage TitleMustBeBetween3And75Chars =
        new(0, "Events title must be between 3 and 75 characters");

    public static readonly ErrorMessage DescriptionMustBeLessThan250Chars =
        new(0, "Events Description must be less than 250 characters");

    public static readonly ErrorMessage StartTimeMustBeBeforeEndTime =
        new(0, "Start time cannot be after end time");

    public static readonly ErrorMessage DurationMustBeMoreThan1Hour =
        new(0, "Duration cannot be less than 1 hour");

    public static readonly ErrorMessage EventDurationMustBeLessThan10Hour =
        new(0, "Events duration cannot be less than 1 hour");

    public static readonly ErrorMessage EventCannotSpanBetween1AmAnd8Am =
        new(0, "Events cannot take place from 1am to 8am");

    public static readonly ErrorMessage StartTimeCannotBeInPast =
        new(0, "Start time must not be on past");

    public static readonly ErrorMessage EventCannotStartBefore8Am =
        new(0, "Events start time cannot be before 8 am");

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
        new(0, "Events duration must be set before making an event ready");


    public static readonly ErrorMessage LocationMustBeSetBeforeMakingAnEventReady =
        new(0, "Location must be set before making an event ready");

    public static readonly ErrorMessage EventInThePastCannotBeReady =
        new(0, "Events with start time at past cannot be made ready");

    public static readonly ErrorMessage ActiveEventCannotBeMadeReady =
        new(0, "Active event cannot be made ready");

    public static readonly ErrorMessage CancelledEventIsUnmodifiable =
        new(0, "Cancelled event cannot be modified");

    public static readonly ErrorMessage CancelledEventCannotBeMadeReady =
        new(0, "Cancelled event cannot be made ready");

    public static readonly ErrorMessage CancelledEventCannotBeActivated =
        new(0, "Cancelled event cannot be activated");

    public static readonly ErrorMessage OnlyActiveEventsCanBeCancelled =
        new(0, "Only active events can be cancelled, if you intend to delete this event, please delete it instead");

    public static readonly ErrorMessage ActiveEventCannotBeDeleted =
        new(0, "Active event cannot be deleted, if you intend to cancel this event, please cancel it instead");


    public static readonly ErrorMessage UnParsableGuid =
        new(0, "The provided guid value is not parsable");

    public static readonly ErrorMessage EmailMustEndWithViaDk = new(0, "Only people with VIA email can register");

    public static readonly ErrorMessage EmailNotInCorrectFormat =
        new(0, "Email is not correct format. Example correct format : text1@via.dk");

    public static readonly ErrorMessage EmailAlreadyAssociatedWithAnotherGuest =
        new(0, "The provided email is already associated with another guest");

    public static readonly ErrorMessage FirstNameMustBeBetween2And25Chars =
        new(0, "The first name must be between 2 and 25 characters");

    public static readonly ErrorMessage LastNameMustBeBetween2And25Chars =
        new(0, "The last name must be between 2 and 25 characters");

    public static readonly ErrorMessage FirstNameCanOnlyContainsLetters =
        new(0, "First name can only contain letters");

    public static readonly ErrorMessage LastNameCanOnlyContainsLetters =
        new(0, "Last name can only contain letters");

    public static readonly ErrorMessage InvalidUrl =
        new(0, "The provided url is not valid");


    public static readonly ErrorMessage InvitationsCanOnlyBeMadeOnReadyOrActiveEvent =
        new(0, "Invitations can only be made on ready or active events");

    public static readonly ErrorMessage OnlyActiveEventsCanBeJoined =
        new(0, "Only active events can be joined");

    public static readonly ErrorMessage EventsCannotBeDeclinedYet =
        new(0, "Events cannot be declined yet");

    public static readonly ErrorMessage EventCannotBeJoinedYet =
        new(0,
            "Events cannot be joined yet");

    public static readonly ErrorMessage PrivateEventCannotBeParticipatedUnlessInvited =
        new(0,
            "Private event cannot be participated unless invited, if you intend to join this event, please request a join instead");
    
    public static readonly ErrorMessage MaximumNumberOfGuestsReached =
        new(0,
            "The event is already full, cannot join the event");

    
    public static readonly ErrorMessage EventHasAlreadyStarted =
        new(0,
            "Cannot join an event after it has started");

    public static readonly ErrorMessage ParticipationOnPastOrOngoingEventsCannotBeCancelled =
        new(0,
            "Participation on past or ongoing events cannot be cancelled");


    public static readonly ErrorMessage CancelledEventsCannotBeJoined =
        new(0,
            "Cancelled events cannot be joined");

    public static readonly ErrorMessage CancelledEventsCannotBeDeclined =
        new(0,
            "Cancelled events cannot be declined");

    public static readonly ErrorMessage InvitationDoesNotExist =
        new(0,
            "Invitation does not exist");


    public static readonly ErrorMessage InvalidLocationName =
        new(0,
            "Invalid Location name");
    
    public static readonly ErrorMessage EventLocationIsNotSet =
        new(0,
            "Location must be set to perform this operation");    
    public static readonly ErrorMessage EventMaxGuestsCannotExceedLocationMaxGuests =
        new(0,
            "Event maximum number of guests cannot exceed location maximum number of guests");



    private ErrorMessage() {
    }

    private ErrorMessage(int value, string displayName) : base(value, displayName) {
    }
}