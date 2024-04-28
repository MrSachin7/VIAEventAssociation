using VIAEventAssociation.Core.AppEntry.Commands;
using VIAEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints.Commands;

public static class CommandEndPointBase {
    public static class WithRequest<TRequest> {
        public abstract class WithResponse<TResponse> : EndpointBase {
            public abstract Task<IResult> HandleAsync(TRequest request);
        }

        public abstract class WithoutResponse : EndpointBase {
            private readonly IDispatcher _dispatcher;

            protected WithoutResponse(IDispatcher dispatcher) {
                _dispatcher = dispatcher;
            }

            public abstract Task<IResult> HandleAsync(TRequest request);


            protected async Task<IResult> ProcessCommandResult<TCommand>(Result<TCommand> commandResult)
                where TCommand : ICommand {
                if (commandResult.IsFailure) {
                    return ConvertErrorToResult(commandResult.Error!);
                }

                return await DispatchCommandAndReturnResult(commandResult.Payload!);
            }

            private async Task<IResult> DispatchCommandAndReturnResult<TCommand>(TCommand command)
                where TCommand : ICommand {
                var result = await _dispatcher.DispatchAsync(command);
                return result.IsSuccess ? Results.Ok() : ConvertErrorToResult(result.Error!);
            }
        }
    }

    public static class WithoutRequest {
        public abstract class WithResponse<TResponse> : EndpointBase {
            public abstract Task<IResult> HandleAsync();
        }

        public abstract class WithoutResponse : EndpointBase {
            public abstract Task<IResult> HandleAsync();
        }
    }
}