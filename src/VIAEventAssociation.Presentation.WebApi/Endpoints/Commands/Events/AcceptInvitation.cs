using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class AcceptInvitation : CommandEndPointBase
    .WithRequest<AcceptInvitationRequest>
    .WithoutResponse{

    public AcceptInvitation(IDispatcher dispatcher) : base(dispatcher) {

    }

    [HttpPatch("events/{Id}/accept-invitation")]
    public override async Task<IResult> HandleAsync(AcceptInvitationRequest request) {
       var commandResult = AcceptInvitationCommand.Create(request.Id, request.RequestBody.InvitationId);
       return await ProcessCommandResult(commandResult);
    }
}

public class AcceptInvitationRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(string InvitationId);
}