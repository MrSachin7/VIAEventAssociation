using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class UpdateDescription : CommandEndPointBase
    .WithRequest<UpdateDescriptionRequest>
    .WithoutResponse {
    public UpdateDescription(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/description")]
    public override async Task<IResult> HandleAsync(UpdateDescriptionRequest request) {
        var commandResult = UpdateEventDescriptionCommand.Create(request.Id, request.RequestBody.Description);
        return await ProcessCommandResult(commandResult);
    }
}

public record UpdateDescriptionRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(string Description);
}