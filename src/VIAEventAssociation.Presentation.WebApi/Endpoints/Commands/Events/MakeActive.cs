using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class MakeActive : CommandEndPointBase
    .WithRequest<MakeActiveRequest>
    .WithoutResponse {
    public MakeActive(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/make-active")]
    public override Task<IResult> HandleAsync(MakeActiveRequest request) {
        var commandResult = MakeEventActiveCommand.Create(request.Id);
        return ProcessCommandResult(commandResult);
    }
    
}

public class MakeActiveRequest {
    [FromRoute] public string Id { get; set; } = null!;
}