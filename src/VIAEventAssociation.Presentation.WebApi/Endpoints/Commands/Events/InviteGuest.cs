using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class InviteGuest : CommandEndPointBase
    .WithRequest<InviteGuestRequest>
    .WithoutResponse{
    public InviteGuest(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/invite-guest")]
    public override Task<IResult> HandleAsync(InviteGuestRequest request) {
        var commandResult = InviteGuestCommand.Create(request.Id, request.RequestBody.GuestId);
        return ProcessCommandResult(commandResult);
    }
    
}

public class InviteGuestRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    
    public record Body(string GuestId);
}