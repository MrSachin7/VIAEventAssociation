using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Commands.Guests;
using VIAEventAssociation.Core.AppEntry.Dispatcher;
using VIAEventAssociation.Core.Domain.Contracts;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands.Guests;

public class Register : CommandEndPointBase
    .WithRequest<RegisterGuestRequest>
    .WithoutResponse {
    private readonly IUniqueEmailChecker _uniqueEmailChecker;

    public Register(IDispatcher dispatcher, IUniqueEmailChecker uniqueEmailChecker) : base(dispatcher) {
        _uniqueEmailChecker = uniqueEmailChecker;
    }

    [HttpPost("guests")]
    public override async Task<IResult> HandleAsync(RegisterGuestRequest request) {
        var commandResult = await RegisterGuestCommand.Create(request.RequestBody.FirstName,
            request.RequestBody.LastName,
            request.RequestBody.ViaEmail,
            request.RequestBody.ProfilePictureUrl,
            _uniqueEmailChecker);
        return await ProcessCommandResult(commandResult);
    }
}

public class RegisterGuestRequest {
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(
        string FirstName,
        string LastName,
        string ViaEmail,
        string ProfilePictureUrl);
}