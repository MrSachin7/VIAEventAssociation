using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class Create : CommandEndPointBase.WithoutRequest.WithResponse<CreateEventResponse> {

    private readonly IDispatcher _dispatcher;

    public Create(IDispatcher dispatcher) {
        _dispatcher = dispatcher;
    }


    [HttpPost("events")]
    public override async Task<IResult> HandleAsync() {
        CreateEventCommand command = CreateEventCommand.Create();
        Result result = await _dispatcher.DispatchAsync(command);
        if (result.IsFailure) {
            return ConvertErrorToResult(result.Error!);
        }

        string createdEventId = command.EventId!.Value.ToString();
        return Results.Created($"/api/events/{createdEventId}", new CreateEventResponse(createdEventId));
    }
}

public record CreateEventResponse(string Id);