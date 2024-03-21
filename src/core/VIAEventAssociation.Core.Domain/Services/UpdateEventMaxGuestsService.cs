using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Services;

public class UpdateEventMaxGuestsService {
    private readonly ILocationRepository _locationRepository;
    private readonly IEventRepository _eventRepository;

    public UpdateEventMaxGuestsService(ILocationRepository locationRepository, IEventRepository eventRepository) {
        _locationRepository = locationRepository;
        _eventRepository = eventRepository;
    }

    public async Task<Result> Handle(EventId eventId, EventMaxGuests maxGuests) {
        VeaEvent? veaEvent = await _eventRepository.FindAsync(eventId);
        if (veaEvent is null) {
            return Error.NotFound(ErrorMessage.EventNotFound(eventId.Value));
        }

        LocationId? locationId = veaEvent.LocationId;
        if (locationId is null) {
            return Error.BadRequest(ErrorMessage.EventLocationIsNotSet);
        }

        Location? location = await _locationRepository.FindAsync(locationId);


        if (location!.LocationMaxGuests.Value < maxGuests.Value) {
            return Error.BadRequest(ErrorMessage.EventMaxGuestsCannotExceedLocationMaxGuests);
        }

        return veaEvent.UpdateMaximumNumberOfGuests(maxGuests);

    }
}