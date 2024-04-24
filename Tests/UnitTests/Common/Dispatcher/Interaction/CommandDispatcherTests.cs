using Microsoft.Extensions.DependencyInjection;
using UnitTests.Fakes.CommandHandler;
using VIAEventAssociation.Core.AppEntry;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Dispatcher.Interaction;

public class CommandDispatcherTests {
    [Fact]
    public async Task CommandDispatcher_WhenCommandIsDispatched_DispatchesThroughCorrectHandler() {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<CreateEventCommand>, TestCreateEventCommandHandler>();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        IDispatcher commandDispatcher = new CommandDispatcher(serviceProvider);
        CreateEventCommand sampleCommand = CreateEventCommand.Create().Payload!;

        // Act
        Result result = await commandDispatcher.DispatchAsync(sampleCommand);

        // Assert
        Assert.True(result.IsSuccess);
        TestCreateEventCommandHandler? handler =
            serviceProvider.GetRequiredService<ICommandHandler<CreateEventCommand>>() as TestCreateEventCommandHandler;
        Assert.NotNull(handler);
        Assert.Equal(1, handler.CallCount);
    }

    // When there are multiple handlers registered...
    [Fact]
    public async Task
        CommandDispatcher_WhenCommandIsDispatched_AndMultipleHandlersAreRegisterd_DispatchesThroughCorrectHandler() {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<CreateEventCommand>, TestCreateEventCommandHandler>();
        serviceCollection.AddScoped<ICommandHandler<MakeEventReadyCommand>, TestMakeEventReadyCommandHandler>();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        IDispatcher commandDispatcher = new CommandDispatcher(serviceProvider);
        CreateEventCommand sampleCommand = CreateEventCommand.Create().Payload!;

        // Act
        Result result = await commandDispatcher.DispatchAsync(sampleCommand);

        // Assert
        Assert.True(result.IsSuccess);
        TestCreateEventCommandHandler? createEventCommandHandler =
            serviceProvider.GetRequiredService<ICommandHandler<CreateEventCommand>>() as TestCreateEventCommandHandler;
        TestMakeEventReadyCommandHandler? makeEventReadyCommandHandler =
            serviceProvider.GetRequiredService<ICommandHandler<MakeEventReadyCommand>>() as
                TestMakeEventReadyCommandHandler;
        Assert.NotNull(createEventCommandHandler);
        Assert.Equal(1, createEventCommandHandler.CallCount);

        Assert.NotNull(makeEventReadyCommandHandler);
        Assert.Equal(0, makeEventReadyCommandHandler.CallCount);
    }

    // Multiple handlers registered and multiple commands dispatched
    [Fact]
    public async Task
        CommandDispatcher_WhenCommandIsDispatched_AndMultipleHandlersAreRegistered_MultipleCommandsAreDispatched_DispatchesThroughCorrectHandlers() {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<CreateEventCommand>, TestCreateEventCommandHandler>();
        serviceCollection.AddScoped<ICommandHandler<MakeEventReadyCommand>, TestMakeEventReadyCommandHandler>();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        IDispatcher commandDispatcher = new CommandDispatcher(serviceProvider);
        CreateEventCommand sampleCommand1 = CreateEventCommand.Create().Payload!;
        MakeEventReadyCommand sampleCommand2 = MakeEventReadyCommand.Create(Guid.NewGuid().ToString()).Payload!;

        // Act
        Result result1 = await commandDispatcher.DispatchAsync(sampleCommand1);
        Result result2 = await commandDispatcher.DispatchAsync(sampleCommand2);


        // Assert
        Assert.True(result1.IsSuccess);
        Assert.True(result2.IsSuccess);
        TestCreateEventCommandHandler? createEventCommandHandler =
            serviceProvider.GetRequiredService<ICommandHandler<CreateEventCommand>>() as TestCreateEventCommandHandler;
        TestMakeEventReadyCommandHandler? makeEventReadyCommandHandler =
            serviceProvider.GetRequiredService<ICommandHandler<MakeEventReadyCommand>>() as
                TestMakeEventReadyCommandHandler;
        Assert.NotNull(createEventCommandHandler);
        Assert.Equal(1, createEventCommandHandler.CallCount);

        Assert.NotNull(makeEventReadyCommandHandler);
        Assert.Equal(1, makeEventReadyCommandHandler.CallCount);
    }

    // When no handler is registered

    [Fact]
    public async Task CommandDispatcher_WhenNoHandlerIsRegistered_ThrowsException() {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        IDispatcher commandDispatcher = new CommandDispatcher(serviceProvider);
        CreateEventCommand sampleCommand = CreateEventCommand.Create().Payload!;

        // Act
        async Task Act() {
            await commandDispatcher.DispatchAsync(sampleCommand);
        }

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(Act);
    }

    // When wrong handler is registered
    [Fact]
    public async Task CommandDispatcher_WhenWrongHandlerIsRegistered_ThrowsException() {
        // Arrange
        ServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<MakeEventReadyCommand>, TestMakeEventReadyCommandHandler>();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        IDispatcher commandDispatcher = new CommandDispatcher(serviceProvider);
        CreateEventCommand sampleCommand = CreateEventCommand.Create().Payload!;

        // Act
        async Task Act() {
            await commandDispatcher.DispatchAsync(sampleCommand);
        }

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(Act);
    }
}                   