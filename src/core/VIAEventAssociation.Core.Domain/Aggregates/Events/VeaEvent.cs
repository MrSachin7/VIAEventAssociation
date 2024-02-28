using VIAEventAssociation.Core.Domain.Aggregates.Events.state;
using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class VeaEvent : Aggregate<EventId> {
    private EventVisibility _visibility;

    private EventTitle _title;

    private EventDescription _description;

    private EventMaxGuests _maxGuests;

    private EventDuration? _duration;

    private IEventStatusState _currentStatusState;


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
            _description = description,
            _visibility = visibility,
            _title = title,
            _maxGuests = maxGuests,
            _currentStatusState = draftStatus
        };
    }

    public Result UpdateTitle(EventTitle title) {
        return _currentStatusState.UpdateTitle(this, title);
    }

    public void UpdateEventDuration(EventDuration eventDuration) {
        _duration = eventDuration;
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

    public Result UpdateMaximumNumberOfGuests(EventMaxGuests maxGuests) {
        return _currentStatusState.UpdateMaxNumberOfGuests(this, maxGuests);
    }


    internal void SetTitle(EventTitle eventTitle) {
        _title = eventTitle;
    }

    internal Result SetStatusToReady() {
        Result result = Result.AsBuilder(ErrorCode.BadRequest)
            .AssertWithError(
                () => !_description.Equals(EventDescription.Default()),
                ErrorMessage.DescriptionMustBeSetBeforeMakingAnEventReady
            )
            .AssertWithError(
                () => !_title.Equals((EventTitle.Default())),
                ErrorMessage.TitleMustBeSetBeforeMakingAnEventReady
            )
            .AssertWithError(
                () => _duration is not null,
                ErrorMessage.EventDurationMustBeSetBeforeMakingAnEventReady
            )
            .AssertWithError(
                () => _duration is not null && _duration.StartDateTime > DateTime.Now,
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
        _description = eventDescription;
    }


    internal void SetVisibility(EventVisibility visibility) {
        _visibility = visibility;
    }

    internal EventVisibility GetVisibility() {
        return _visibility;
    }


    // Todo: Location logics will be implemented later..


    internal Result SetMaximumNumberOfGuests(EventMaxGuests maxGuests) {
        if (_maxGuests.Value < maxGuests.Value) {
            return Error.BadRequest(ErrorMessage.ActiveEventCannotReduceMaxGuests);
        }

        _maxGuests = maxGuests;
        return Result.Success();
    }

    internal EventMaxGuests GetMaximumNumberOfGuests() => _maxGuests;


    internal void SetEventStatusState(IEventStatusState statusState) {
        _currentStatusState = statusState;
    }

    internal EventDescription GetEventDescription() => _description;

    internal EventTitle GetEventTitle() => _title;

    private void SetStatus(IEventStatusState statusState) {
        _currentStatusState = statusState;
    }
}