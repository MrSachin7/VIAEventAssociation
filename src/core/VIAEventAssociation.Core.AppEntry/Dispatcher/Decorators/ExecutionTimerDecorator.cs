using System.Diagnostics;
using Serilog;
using VIAEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Dispatcher.Decorators;

public class ExecutionTimerDecorator : IDispatcher{
    private readonly IDispatcher _nextDispatcher;
    private readonly ILogger _logger;

    public ExecutionTimerDecorator(IDispatcher nextDispatcher, ILogger logger) {
       _nextDispatcher = nextDispatcher;
       _logger = logger;
    }

    public async Task<Result> DispatchAsync(ICommand command) {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        Result result = await _nextDispatcher.DispatchAsync(command);
        stopwatch.Stop();

        if (result.IsSuccess) {
            _logger.Information("Command {Name} executed successfully in {StopwatchElapsedMilliseconds} ms", command.GetType().Name, stopwatch.ElapsedMilliseconds);
        }
        else {
            _logger.Error("Command {Name} failed after {StopwatchElapsedMilliseconds} ms with reason: {Reason}", command.GetType().Name, stopwatch.ElapsedMilliseconds, result.Error);
        }
        return result;
    }
}