using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Events.state;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class VeaEvent : Aggregate<EventId> {
    internal EventDuration? Duration { get; private set; }

    private IEventStatusState _currentStatusState;

    internal EventStatus Status => _currentStatusState.CurrentStatus();
    internal EventVisibility Visibility { get; private set; }

    internal EventTitle Title { get; private set; }

    internal EventDescription Description { get; private set; }

    internal EventMaxGuests MaxGuests { get; private set; }

    internal ICollection<EventInvitation> EventInvitations { get; private set; }

    internal ISet<GuestId> IntendedParticipants { get; private set; }

    // Todo: ask if this is the way of handling systemtime
    private ISystemTime _systemTime;


    private VeaEvent() {
        _systemTime = new DefaultSystemTime();
        EventInvitations = new List<EventInvitation>();
        IntendedParticipants = new HashSet<GuestId>();
    }

    public static VeaEvent Empty() {
        EventId id = EventId.New();
        IEventStatusState draftStatus = DraftStatusState.GetInstance();
        // Todo : ask troels if this default logic should be here or in the eventmaxguests
        EventMaxGuests maxGuests = EventMaxGuests.Default();
        EventDescription description = EventDescription.Default();
        EventVisibility visibility = EventVisibility.Private;
        EventTitle title = EventTitle.Default();

        return new VeaEvent() {
            Id = id,
            Description = description,
            Visibility = visibility,
            Title = title,
            MaxGuests = maxGuests,
            _currentStatusState = draftStatus
        };
    }

    public Result UpdateTitle(EventTitle title) {
        return _currentStatusState.UpdateTitle(this, title);
    }

    public Result UpdateEventDuration(EventDuration eventDuration) {
        return _currentStatusState.UpdateEventDuration(this, eventDuration);
    }

    public Result UpdateDescription(EventDescription eventDescription) {
        return _currentStatusState.UpdateDescription(this, eventDescription);
    }

    public Result MakePublic() {
        return _currentStatusState.MakePublic(this);
    }

    public Result MakePrivate() {
        return _currentStatusState.MakePrivate(this);
    }

    public Result MakeReady() {
        return _currentStatusState.MakeReady(this);
    }

    public Result MakeActive() {
        return _currentStatusState.MakeActive(this);
    }

    // TODO: there is ntg regarding this in the use case desc.
    public Result MakeCancelled() {
        return _currentStatusState.MakeCancelled(this);
    }

    public Result UpdateMaximumNumberOfGuests(EventMaxGuests maxGuests) {
        return _currentStatusState.UpdateMaxNumberOfGuests(this, maxGuests);
    }

    public Result InviteGuest(EventInvitation invitation) {
        // Max number of guests reached
        if (GetNumberOfParticipants() >= MaxGuests.Value) {
            return Error.BadRequest(ErrorMessage.MaximumNumberOfGuestsReached);
        }

        return _currentStatusState.InviteGuest(this, invitation);
    }

    //  Todo: Should I make sure that the guestId exists ?
    public Result ParticipateGuest(GuestId guestId) {
        // If not public, fail
        if (Visibility.Equals(EventVisibility.Private)) {
            return Error.BadRequest(ErrorMessage.PrivateEventCannotBeParticipatedUnlessInvited);
        }

        // Max number of guests reached
        if (IsFull()) {
            return Error.BadRequest(ErrorMessage.MaximumNumberOfGuestsReached);
        }

        return _currentStatusState.ParticipateGuest(this, guestId);
    }

    // Todo: So, if a guest accepted an invitation, should this also remove that accepted invitation ?
    public Result CancelGuestParticipation(GuestId guestId) {
        // Todo : What happens if this is called on a draft event where duration is null ?, now is that a business logic ?? or the null pointer exception is fine ?

        // No real state logic, so no need to call _currentStatusState
        if (Duration!.StartDateTime < _systemTime.CurrentTime()) {
            return Error.BadRequest(ErrorMessage.ParticipationOnPastOrOngoingEventsCannotBeCancelled);
        }

        IntendedParticipants.Remove(guestId);
        return Result.Success();
    }

    // Todo: Does this belong here or on GuestAggregate ??
    public Result AcceptInvitation(EventInvitationId invitationId) {
        // Todo: So only pending invitations ? what about the declined invitation ?
        bool invitationExists = EventInvitations.Any(invitation =>
            invitation.Id.Equals(invitationId) && invitation.Status.Equals(JoinStatus.Pending));
        if (!invitationExists) {
            return Error.NotFound(ErrorMessage.InvitationDoesNotExist);
        }

        if (IsFull()) {
            return Error.BadRequest(ErrorMessage.MaximumNumberOfGuestsReached);
        }

        return _currentStatusState.AcceptInvitation(this, invitationId);
    }

    public Result DeclineInvitation(EventInvitationId invitationId) {
        bool invitationExists = EventInvitations.Any(invitation =>
            invitation.Id.Equals(invitationId));
        if (!invitationExists) {
            return Error.NotFound(ErrorMessage.InvitationDoesNotExist);
        }

        return _currentStatusState.DeclineInvitation(this, invitationId);
    }

    internal void MakeInvitationAccepted(EventInvitationId invitationId) {
        // Here we are sure that the invitation exists
        EventInvitation invitation = EventInvitations.First(invitation =>
            invitation.Id.Equals(invitationId) && invitation.Status.Equals(JoinStatus.Pending));
        invitation.Accept();
    }

    public void MakeInvitationDeclined(EventInvitationId invitationId) {
        // Here we are sure that the invitation exists
        EventInvitation invitation = EventInvitations.First(invitation =>
            invitation.Id.Equals(invitationId));
        invitation.Decline();
    }


    internal Result AddIntendedParticipant(GuestId guestId) {
        // Only active event can call this method, so we are sure that the duration is not null
        if (Duration!.StartDateTime < _systemTime.CurrentTime()) {
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

    internal Result SetStatusToReady() {
        Result result = Result.AsBuilder(ErrorCode.BadRequest)
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
                () => Duration is not null && Duration.StartDateTime > DateTime.Now,
                ErrorMessage.EventInThePastCannotBeReady
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


    // Todo: Location logics will be implemented later..
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
        _currentStatusState = statusState;
    }


    private int GetNumberOfParticipants() {
        return IntendedParticipants.Count + GetNumberOfAcceptedInvitations();
    }

    private int GetNumberOfAcceptedInvitations() {
        return EventInvitations.Count(invitation => invitation.Status.Equals(JoinStatus.Accepted));
    }

 

   
}