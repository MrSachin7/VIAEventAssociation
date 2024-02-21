namespace ViaEventAssociation.Core.Tools.OperationResult;

public class Result {


    public Error? Error { get; init; }
    public bool IsFailure => Error is not null;

    // Implicit converter
    public static implicit operator Result(Error error)
        => new Result {Error = error};

    public bool HasValidationErrors => Error?.IsValidationError ?? false;
}

public class Result<T> : Result {
    public T? Payload { get; init; } 

    public static implicit operator Result<T>(Error error)
        => new Result<T> {Error = error};

    public static implicit operator Result<T>(T payload)
        => new Result<T> {Payload = payload};
}