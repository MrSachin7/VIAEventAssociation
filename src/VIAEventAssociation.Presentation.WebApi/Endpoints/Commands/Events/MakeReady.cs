using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class MakeReady : CommandEndPointBase
    .WithRequest<MakeReadyRequest>
    .WithoutResponse {
    public MakeReady(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/make-ready")]
    public override Task<IResult> HandleAsync(MakeReadyRequest request) {
        var commandResult = MakeEventReadyCommand.Create(request.Id);
        return ProcessCommandResult(commandResult);
    }
    
}

public class MakeReadyRequest {
    [FromRoute] public string Id { get; set; } = null!;
}