using Moq;
using VIAEventAssociation.Core.AppEntry.Commands;
using VIAEventAssociation.Core.AppEntry.Commands.Events;
using VIAEventAssociation.Core.AppEntry.Dispatcher;
using VIAEventAssociation.Core.AppEntry.Dispatcher.Decorators;
using VIAEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Dispatcher.Decorators;

public class TransactionDecoratorTests {
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IDispatcher> _commandDispatcherMock = new();


    [Fact]
    public async Task TransactionDecorator_WhenSuccessResult_CommitsTransaction() {
        // Arrange success result
        TransactionDecorator sut = new TransactionDecorator(_commandDispatcherMock.Object, _unitOfWorkMock.Object);
        _commandDispatcherMock.Setup(mock => mock.DispatchAsync(It.IsAny<ICommand>()))
            .ReturnsAsync(Result.Success);
        ICommand sampleCommand = CreateEventCommand.Create().Payload!;

        // Act
        await sut.DispatchAsync(sampleCommand);

        // Assert that the transaction is committed
        _commandDispatcherMock.Verify(mock => mock.DispatchAsync(It.IsAny<ICommand>()), Times.Once);
        _unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(), Times.Once);

    }

    [Fact]
    public async Task TransactionDecorator_WhenFailureResult_DoesNotCommitTransaction() {
        // Arrange success result
        TransactionDecorator sut = new TransactionDecorator(_commandDispatcherMock.Object, _unitOfWorkMock.Object);
        Result sampleFailureResult = Error.NotFound(ErrorMessage.EventLocationIsNotSet);

        _commandDispatcherMock.Setup(mock => mock.DispatchAsync(It.IsAny<ICommand>()))
            .ReturnsAsync(sampleFailureResult);
        ICommand sampleCommand = CreateEventCommand.Create().Payload!;

        // Act
        await sut.DispatchAsync(sampleCommand);

        // Assert that the transaction is not committed
        _commandDispatcherMock.Verify(mock => mock.DispatchAsync(It.IsAny<ICommand>()), Times.Once);
        _unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(), Times.Never);

    }




}