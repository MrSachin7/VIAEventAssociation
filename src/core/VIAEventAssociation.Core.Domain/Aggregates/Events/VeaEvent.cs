using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Events.state;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class VeaEvent : Aggregate<EventId> {
    internal EventDuration? Duration { get; private set; }

    internal IEventStatusState CurrentStatusState;

    internal EventStatus Status => CurrentStatusState.CurrentStatus();
    internal EventVisibility Visibility { get; private set; }

    internal EventTitle Title { get; private set; }

    internal EventDescription Description { get; private set; }

    internal EventMaxGuests MaxGuests { get; private set; }

    internal ICollection<EventInvitation> EventInvitations { get; private set; }

    internal ICollection<EventParticipation> IntendedParticipants { get; private set; }

    internal Location? Location { get; private set; }

    private VeaEvent() {
        // Efc needs this
    }


    private VeaEvent(EventId id, EventDescription description, EventVisibility visibility, EventTitle title
        , EventMaxGuests maxGuests, IEventStatusState currentStatusState) {
        Id = id;
        Description = description;
        Visibility = visibility;
        Title = title;
        MaxGuests = maxGuests;
        CurrentStatusState = currentStatusState;
        EventInvitations = new List<EventInvitation>();
        IntendedParticipants = new List<EventParticipation>();
    }

    public static VeaEvent Empty(EventId id) {
        IEventStatusState draftStatus = DraftStatusState.GetInstance();
        EventMaxGuests maxGuests = EventMaxGuests.Default();
        EventDescription description = EventDescription.Default();
        EventVisibility visibility = EventVisibility.Private;
        EventTitle title = EventTitle.Default();

        return new VeaEvent(id, description, visibility, title, maxGuests, draftStatus);
    }

    public Result UpdateEventTitle(EventTitle title) {
        return CurrentStatusState.UpdateTitle(this, title);
    }

    public Result UpdateEventDuration(EventDuration eventDuration) {
        return CurrentStatusState.UpdateEventDuration(this, eventDuration);
    }

    public Result UpdateEventDescription(EventDescription eventDescription) {
        return CurrentStatusState.UpdateDescription(this, eventDescription);
    }

    public Result MakePublic() {
        return CurrentStatusState.MakePublic(this);
    }

    public Result MakePrivate() {
        return CurrentStatusState.MakePrivate(this);
    }

    public Result MakeReady(ISystemTime systemTime) {
        return CurrentStatusState.MakeReady(this, systemTime);
    }

    public Result MakeActive(ISystemTime systemTime) {
        return CurrentStatusState.MakeActive(this, systemTime);
    }

    public Result MakeCancelled() {
        return CurrentStatusState.MakeCancelled(this);
    }


    public Result CancelGuestParticipation(Guest guest, ISystemTime systemTime) {
        // No real state logic, so no need to call CurrentStatusState

        if (Duration is null || Duration.StartDateTime < systemTime.CurrentTime()) {
            return Error.BadRequest(ErrorMessage.ParticipationOnPastOrOngoingEventsCannotBeCancelled);
        }

        IntendedParticipants.Remove(guest.Id);
        return Result.Success();
    }

    public Result AcceptInvitation(EventInvitationId invitationId) {
        if (IsFull()) {
            return Error.BadRequest(ErrorMessage.MaximumNumberOfGuestsReached);
        }

        EventInvitation? invitationToAccept = EventInvitations.FirstOrDefault(invitation =>
            invitation.Id.Equals(invitationId) && invitation.Status.Equals(JoinStatus.Pending));


        if (invitationToAccept is null) {
            return Error.NotFound(ErrorMessage.InvitationDoesNotExist);
        }


        return CurrentStatusState.AcceptInvitation(this, invitationToAccept);
    }

    public Result DeclineInvitation(EventInvitationId invitationId) {
        EventInvitation? invitationToDecline = EventInvitations
            .FirstOrDefault(invitation => invitation.Id.Equals(invitationId));


        if (invitationToDecline is null) {
            return Error.NotFound(ErrorMessage.InvitationDoesNotExist);
        }

        return CurrentStatusState.DeclineInvitation(this, invitationToDecline);
    }

    public Result UpdateLocation(Location location) {
        return CurrentStatusState.UpdateLocation(this, location);
    }

    public Result ParticipateGuest(Guest guest, ISystemTime systemTime) {
        // If not public, fail
        if (Visibility.Equals(EventVisibility.Private)) {
            return Error.BadRequest(ErrorMessage.PrivateEventCannotBeParticipatedUnlessInvited);
        }

        // Max number of guests reached
        if (IsFull()) {
            return Error.BadRequest(ErrorMessage.MaximumNumberOfGuestsReached);
        }

        return CurrentStatusState.ParticipateGuest(this, guest.Id, systemTime);
    }


    public Result InviteGuest(EventInvitation invitation) {
        // Max number of guests reached
        if (GetNumberOfParticipants() >= MaxGuests.Value) {
            return Error.BadRequest(ErrorMessage.MaximumNumberOfGuestsReached);
        }

        return CurrentStatusState.InviteGuest(this, invitation);
    }

    public Result UpdateMaximumNumberOfGuests(EventMaxGuests maxGuests) {
        if (Location is null) {
            return Error.BadRequest(ErrorMessage.EventLocationIsNotSet);
        }

        int maxGuestsAllowedByLocation = Location.LocationMaxGuests.Value;
        int attemptedMaxGuests = maxGuests.Value;

        if (attemptedMaxGuests > maxGuestsAllowedByLocation) {
            return Error.BadRequest(ErrorMessage.EventMaxGuestsCannotExceedLocationMaxGuests);
        }

        return CurrentStatusState.UpdateMaxNumberOfGuests(this, maxGuests);
    }


    internal void SetLocation(Location location) {
        Location = location;
    }

    internal void MakeInvitationDeclined(EventInvitation invitation) {
        invitation.Decline();
    }


    internal void MakeInvitationAccepted(EventInvitation invitation) {
        // Here we are sure that the invitation exists
        invitation.Accept();
    }


    internal Result AddIntendedParticipant(GuestId guestId, ISystemTime systemTime) {
        // Only active event can call this method, so we are sure that the duration is not null
        if (Duration!.StartDateTime < systemTime.CurrentTime()) {
            return Error.BadRequest(ErrorMessage.EventHasAlreadyStarted);
        }

        IntendedParticipants.Add(guestId);
        return Result.Success();
    }

    internal void AddInvitation(EventInvitation invitation) {
        EventInvitations.Add(invitation);
    }

    internal void SetTitle(EventTitle eventTitle) {
        Title = eventTitle;
    }

    internal Result SetStatusToReady(ISystemTime systemTime) {
        Result result = Result.ToBuilder(ErrorCode.BadRequest)
            .AssertWithError(
                () => !Description.Equals(EventDescription.Default()),
                ErrorMessage.DescriptionMustBeSetBeforeMakingAnEventReady
            )
            .AssertWithError(
                () => !Title.Equals((EventTitle.Default())),
                ErrorMessage.TitleMustBeSetBeforeMakingAnEventReady
            )
            .AssertWithError(
                () => Duration is not null,
                ErrorMessage.EventDurationMustBeSetBeforeMakingAnEventReady
            )
            .AssertWithError(
                () => Duration is not null && Duration.StartDateTime > systemTime.CurrentTime(),
                ErrorMessage.EventInThePastCannotBeReady
            )
            .AssertWithError(
                () => Location is not null,
                ErrorMessage.LocationMustBeSetBeforeMakingAnEventReady
            )
            .Build();

        if (result.IsFailure) {
            return result;
        }

        SetStatus(ReadyStatusState.GetInstance());
        return Result.Success();
    }

    internal void SetStatusToCancelled() {
        SetStatus(CancelledStatusState.GetInstance());
    }

    internal void SetStatusToDraft() {
        SetStatus(DraftStatusState.GetInstance());
    }

    internal void SetStatusToActive() {
        SetStatus(ActiveStatusState.GetInstance());
    }

    internal void SetDescription(EventDescription eventDescription) {
        Description = eventDescription;
    }


    internal void SetVisibility(EventVisibility visibility) {
        Visibility = visibility;
    }


    internal Result SetMaximumNumberOfGuests(EventMaxGuests maxGuests) {
        MaxGuests = maxGuests;
        return Result.Success();
    }

    internal void SetEventDuration(EventDuration eventDuration) {
        Duration = eventDuration;
    }

    internal bool IsFull() {
        return GetNumberOfParticipants() >= MaxGuests.Value;
    }

    private void SetStatus(IEventStatusState statusState) {
        CurrentStatusState = statusState;
    }

    private int GetNumberOfParticipants() {
        return IntendedParticipants.Count + GetNumberOfAcceptedInvitations();
    }

    private int GetNumberOfAcceptedInvitations() {
        return EventInvitations.Count(invitation => invitation.Status.Equals(JoinStatus.Accepted));
    }

    public override bool Equals(object? obj) {
        VeaEvent? other = obj as VeaEvent;
        if (other is null) {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}