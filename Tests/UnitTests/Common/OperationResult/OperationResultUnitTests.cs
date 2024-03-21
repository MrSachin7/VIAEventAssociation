using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.OperationResult;

public class OperationResultUnitTests {

    [Fact]
    public void ImplicitConversion_FromErrorToResult()
    {
        // Arrange
        Error error = Error.NotFound(ErrorMessage.TitleMustBeBetween3And75Chars);

        // Act
        Result result = error;

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromErrorToResultT()
    {
        // Arrange
        Error error = Error.NotFound(ErrorMessage.TitleMustBeBetween3And75Chars);

        // Act
        Result<int> result = error;

        // Assert
        Assert.NotNull(result.Error);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromPayloadToResultT()
    {
        // Arrange
        int payloadValue = 42;

        // Act
        Result<int> result = payloadValue;

        // Assert
        Assert.Equal(payloadValue, result.Payload);
    }
    
    [Fact]
    public void Result_IsFailure_WhenErrorIsNotNull() {
        // Arrange
        // Example usage of implicit conversion
        Result result = Error.NotFound(ErrorMessage.TitleMustBeBetween3And75Chars);

        // Act and Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Result_IsNotFailure_WhenErrorIsNull()
    {
        // Arrange
        Result<int> result = 14;

        // Act & Assert
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void ResultT_IsFailure_WhenErrorIsNotNull()
    {
        // Arrange
        Result<int> result = Error.BadRequest(ErrorMessage.TitleMustBeBetween3And75Chars);

        // Act & Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void ResultT_IsNotFailure_WhenErrorIsNull()
    {
        // Arrange
        Result<int> result = 42;

        // Act & Assert
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Combine_BothFailures_CombinesErrorMessages() {
        // Arrange
        Result result1 = Error.BadRequest(ErrorMessage.TitleMustBeBetween3And75Chars);
        Result result2 = Error.BadRequest(ErrorMessage.EventHasAlreadyStarted);

        // Act
        var combinedResult = result1.Combine(result2);

        // Assert
        Assert.True(combinedResult.IsFailure);
        Assert.NotNull(combinedResult.Error);

        const int finalErrorCount = 2;
        Assert.Equal(finalErrorCount, combinedResult.Error!.Messages.Count);
        Assert.Contains(ErrorMessage.TitleMustBeBetween3And75Chars, combinedResult.Error!.Messages);
        Assert.Contains(ErrorMessage.EventHasAlreadyStarted, combinedResult.Error!.Messages);
    }

    [Fact]
    public void Combine_FailureWithSuccess_ReturnsTheFailureResult() {
        // Arrange
        Result result1 = Error.BadRequest(ErrorMessage.TitleMustBeBetween3And75Chars);
        Result result2 = Result.Success();

        // Act
        Result combinedResult =result1.Combine(result2);

        Assert.True(combinedResult.IsFailure);
        Assert.Equal(result1, combinedResult);
    }

    [Fact]
    public void Combine_SuccessWithFailure_ReturnsTheFailureResult() {
        // Arrange
        Result result1 = Result.Success();
        Result result2 = Error.BadRequest(ErrorMessage.TitleMustBeBetween3And75Chars);

        // Act
        Result combinedResult =result1.Combine(result2);

        Assert.True(combinedResult.IsFailure);
        Assert.Equal(result2, combinedResult);
    }

    [Fact]
    public void Combine_SuccessWithSuccess_ReturnsTheSuccessResult() {
        // Arrange
        Result result1 = Result.Success();
        Result result2 = Result.Success();

        // Act
        Result combinedResult =result1.Combine(result2);

        Assert.True(combinedResult.IsSuccess);
    }

    
}