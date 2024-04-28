using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Events;

public class UpdateDuration : CommandEndPointBase
    .WithRequest<UpdateDurationRequest>
    .WithoutResponse {
    private readonly ISystemTime _systemTime;

    public UpdateDuration(IDispatcher dispatcher, ISystemTime systemTime) : base(dispatcher) {
        _systemTime = systemTime;
    }

    public override async Task<IResult> HandleAsync(UpdateDurationRequest request) {
        DateTime startDateTime = DateTime.ParseExact(request.RequestBody.StartDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        DateTime endDateTime = DateTime.ParseExact(request.RequestBody.EndDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var commandResult = UpdateEventDurationCommand.Create(request.Id, startDateTime, endDateTime, _systemTime);
        return await ProcessCommandResult(commandResult);
    }
}

public record UpdateDurationRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(string StartDateTime, string EndDateTime);
}