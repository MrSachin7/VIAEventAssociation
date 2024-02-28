using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.OperationResult;

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

    
}