using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class CancelEventParticipation : CommandEndPointBase
    .WithRequest<CancelEventParticipationRequest>
    .WithoutResponse {

    public CancelEventParticipation(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/cancel-participation")]
    public override async Task<IResult> HandleAsync(CancelEventParticipationRequest request) {
        var commandResult = CancelEventParticipationCommand.Create(request.Id, request.RequestBody.GuestId);
        return await ProcessCommandResult(commandResult);
    }
}


public class CancelEventParticipationRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    
    public record Body(string GuestId);
}