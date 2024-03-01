using VIAEventAssociation.Core.Domain.Aggregates.Events.state;
using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class VeaEvent : Aggregate<EventId> {
    internal  EventDuration? Duration { get; private set; }

    private IEventStatusState _currentStatusState;

    internal EventStatus Status => _currentStatusState.CurrentStatus();
    internal EventVisibility Visibility { get; private set; }

    internal EventTitle Title { get; private set; }

    internal EventDescription Description { get; private set; }

    internal EventMaxGuests MaxGuests { get; private set; }


    private VeaEvent() {
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

    internal EventVisibility GetVisibility() {
        return Visibility;
    }


    // Todo: Location logics will be implemented later..
    internal Result SetMaximumNumberOfGuests(EventMaxGuests maxGuests) {
        MaxGuests = maxGuests;
        return Result.Success();
    }

    internal void SetEventDuration(EventDuration eventDuration) {
        Duration = eventDuration;
    }


    private void SetStatus(IEventStatusState statusState) {
        _currentStatusState = statusState;
    }


}