using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;
using VIAEventAssociation.Core.Domain.temp;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Services;

// Todo: Not sure if this is the way or should the event store location instead of locationId
public class UpdateEventMaxGuestsService {
    private readonly ILocationRepository _locationRepository;

    public UpdateEventMaxGuestsService(ILocationRepository locationRepository) {
        _locationRepository = locationRepository;
    }

    public Result Handle(VeaEvent veaEvent, EventMaxGuests maxGuests) {
        LocationId? locationId = veaEvent.LocationId;
        if (locationId is null) {
            return Error.BadRequest(ErrorMessage.EventLocationIsNotSet);
        }
        Location location = _locationRepository.FindById(locationId.GetValue());
        if (location.LocationMaxGuests.Value < maxGuests.Value) {
            return Error.BadRequest(ErrorMessage.EventMaxGuestsCannotExceedLocationMaxGuests);
        }

        return veaEvent.UpdateMaximumNumberOfGuests(maxGuests);

    }
}