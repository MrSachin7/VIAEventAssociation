using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class MakePrivate : CommandEndPointBase
    .WithRequest<MakePrivateRequest>
    .WithoutResponse {
    public MakePrivate(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/make-private")]
    public override Task<IResult> HandleAsync(MakePrivateRequest request) {
        var commandResult = MakeEventPrivateCommand.Create(request.Id);
        return ProcessCommandResult(commandResult);
    }
    
}

public class MakePrivateRequest {
    [FromRoute] public string Id { get; set; } = null!;
}