using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.OperationResult;

public class OperationResultUnitTests {

    [Fact]
    public void ImplicitConversion_FromErrorToResult()
    {
        // Arrange
        Error error = Error.From(ErrorCode.Forbidden, "ErrorMessage");

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
        Error error =  Error.From(ErrorCode.Forbidden, "ErrorMessage");

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
        Result result = Error.From(ErrorCode.BadRequest, "Bad request");

        // Act and Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Result_IsNotFailure_WhenErrorIsNull()
    {
        // Arrange
        var result = new Result();

        // Act & Assert
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void ResultT_IsFailure_WhenErrorIsNotNull()
    {
        // Arrange
        Result<int> result = Error.From(ErrorCode.BadRequest, "Bad request");

        // Act & Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void ResultT_IsNotFailure_WhenErrorIsNull()
    {
        // Arrange
        var result = new Result<int> { Payload = 42 };

        // Act & Assert
        Assert.False(result.IsFailure);
    }

    
}