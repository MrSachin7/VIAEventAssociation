using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class MakePublic : CommandEndPointBase
    .WithRequest<MakePublicRequest>
    .WithoutResponse {
    public MakePublic(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/make-public")]
    public override Task<IResult> HandleAsync(MakePublicRequest request) {
        var commandResult = MakeEventPublicCommand.Create(request.Id);
        return ProcessCommandResult(commandResult);
    }
    
}

public class MakePublicRequest {
    [FromRoute] public string Id { get; set; } = null!;
}