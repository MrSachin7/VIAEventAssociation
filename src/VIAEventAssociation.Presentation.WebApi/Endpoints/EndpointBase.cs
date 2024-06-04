using Microsoft.AspNetCore.Mvc;
using VIAEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints;

[ApiController, Route("api")]
public class EndpointBase : ControllerBase {

    protected IResult ConvertErrorToResult(Error error) {
        int statusCode = error.ErrorCode.Value;
        ProblemDetails problemDetails = new ProblemDetails {
            Status = statusCode,
            Title = error.ErrorCode.DisplayName,
        };

        if (statusCode != StatusCodes.Status500InternalServerError) {
            problemDetails.Extensions = new Dictionary<string, object?> {
                {"errors", new[] {error.GetErrorMessages()}}
            };
        }

        return Results.Problem(problemDetails);
    }

}