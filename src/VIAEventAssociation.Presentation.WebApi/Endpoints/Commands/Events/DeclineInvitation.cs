using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class DeclineInvitation : CommandEndPointBase
    .WithRequest<DeclineInvitationRequest>
    .WithoutResponse{
    public DeclineInvitation(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/decline-invitation")]
    public override async Task<IResult> HandleAsync(DeclineInvitationRequest request) {
        var commandResult = DeclineInvitationCommand.Create(request.Id, request.RequestBody.InvitationId);
        return await ProcessCommandResult(commandResult);

    }
}

public class DeclineInvitationRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;
    public record Body(string InvitationId);
}