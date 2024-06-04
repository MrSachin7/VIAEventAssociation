using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.QueryContracts.Dispatcher;
using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Query;

public class EventDetails : QueryEndpointBase
    .WithRequest<EventDetailsRequest>
    .WithResponse<EventDetailsResponse> {
    private readonly IMapper _mapper;
    private readonly IQueryDispatcher _dispatcher;

    public EventDetails(IMapper mapper, IQueryDispatcher dispatcher) {
        _mapper = mapper;
        _dispatcher = dispatcher;
    }

    [HttpGet, Route("events/{EventId}")]
    public override async Task<IResult> HandleAsync(EventDetailsRequest request) {
        var query = _mapper.Map<EventDetailsQuery.Query>(request);
        var answer = await _dispatcher.DispatchAsync(query);
        if (answer.IsFailure) {
            return ConvertErrorToResult(answer.Error!);
        }
        var response = _mapper.Map<EventDetailsResponse>(answer.Payload!);
        return Results.Ok(response);
    }
}

public class EventDetailsRequest {
    [FromRoute] public string EventId { get; set; } = null!;

    [FromQuery] public int GuestPageNumber { get; set; } = 1;

    [FromQuery] public int GuestsPerPage { get; set; } = 10;
}

public record EventDetailsResponse {
    public Event VeaEvent { get; set; } = null!;
    public GuestList Guests { get; set; } = null!;

    public record Event(
        string EventId,
        string Title,
        string Description,
        string? Date,
        string? StartTime,
        string Visibility,
        int NumberOfAttendees,
        int MaxGuests,
        string? Location);

    public record GuestList(
        ICollection<Guest> Participants,
        int TotalGuestPages);

    public record Guest(
        string Id,
        string Name,
        string? ProfilePictureUrl);
}