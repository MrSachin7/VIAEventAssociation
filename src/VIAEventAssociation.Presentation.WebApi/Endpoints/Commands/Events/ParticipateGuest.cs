using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class ParticipateGuest : CommandEndPointBase
    .WithRequest<ParticipateGuestRequest>
    .WithoutResponse {
    public ParticipateGuest(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPost("events/{Id}/participate-guest")]
    public override Task<IResult> HandleAsync(ParticipateGuestRequest request) {
        var commandResult = ParticipateGuestCommand.Create(request.Id, request.RequestBody.GuestId);
        return ProcessCommandResult(commandResult);
    }
    
}

public class ParticipateGuestRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(string GuestId);
}