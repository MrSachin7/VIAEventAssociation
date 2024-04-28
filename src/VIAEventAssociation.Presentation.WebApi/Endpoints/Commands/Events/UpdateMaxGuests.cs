using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class UpdateMaxGuests : CommandEndPointBase
    .WithRequest<UpdateMaxGuestsRequest>
    .WithoutResponse {

    public UpdateMaxGuests(IDispatcher dispatcher) : base(dispatcher) {
    }

    [HttpPatch("events/{Id}/max-guests")]
    public override async Task<IResult> HandleAsync(UpdateMaxGuestsRequest request) {
        var commandResult = UpdateEventMaxGuestsCommand.Create(request.Id, request.RequestBody.MaxGuests);
        return await ProcessCommandResult(commandResult);
    }
}

public class UpdateMaxGuestsRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(int MaxGuests);
}