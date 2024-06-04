using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class UpdateTitle : CommandEndPointBase
    .WithRequest<UpdateTitleRequest>
    .WithoutResponse {


    public UpdateTitle(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/title")]
    public override async Task<IResult> HandleAsync(UpdateTitleRequest request) {
        var commandResult = UpdateEventTitleCommand.Create(request.Id, request.RequestBody.Title);
        return await ProcessCommandResult(commandResult);
    }
}

public class UpdateTitleRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(string Title);
}