namespace ViaEventAssociation.Core.Tools.OperationResult;

public class Result {
    public Error? Error { get; init; }
    public bool IsFailure => Error is not null;
    public bool IsSuccess => !IsFailure;

    protected Result() {
    }

    public static Result Success() => new Result();

    public static implicit operator Result(Error error)
        => new Result {Error = error};

    public static Result.Builder AsBuilder(ErrorCode errorCodeIfFails) => new Builder(errorCodeIfFails);

    // Implicit converter


    public class Builder {
        protected readonly ErrorCode ErrorCodeIfFailure;
        protected List<ErrorMessage>? ErrorMessages;

        internal Builder(ErrorCode errorCodeIfFailure) {
            ErrorCodeIfFailure = errorCodeIfFailure;
        }

        public Builder AssertWithError(Func<bool> condition, ErrorMessage errorMessageToDisplayOnConditionFail) {
            if (!condition()) {
                AddErrorMessage(errorMessageToDisplayOnConditionFail);
            }

            return this;
        }

        protected void AddErrorMessage(ErrorMessage errorMessage) {
            if (ErrorMessages is null) {
                ErrorMessages = new List<ErrorMessage>() {errorMessage};
            }
            else {
                ErrorMessages.Add(errorMessage);
            }
        }

        public virtual Result Build() {
            if (ErrorMessages is null) {
                return Result.Success();
            }

            return new Error(ErrorCodeIfFailure, ErrorMessages);
        }
    }
}

public class Result<T> : Result {
    private readonly T? _payload;

    private Result() : base() {
    }

    public T? Payload {
        get {
            AssertAccessible();
            return _payload;
        }
        init {
            AssertAccessible();
            _payload = value;
        }
    }


    public static Result<T>.Builder AsBuilder(ErrorCode errorCodeIfFailure, T payLoadIfSuccess) =>
        new(errorCodeIfFailure, payLoadIfSuccess);

    public static implicit operator Result<T>(Error error)
        => new Result<T> {Error = error};

    public static implicit operator Result<T>(T payload)
        => new Result<T> {Payload = payload};

    private void AssertAccessible() {
        if (Error is not null) {
            throw new Exception("Payload is not allowed on an error result");
        }
    }

    
 

    public new class Builder: Result.Builder {
        private readonly T _payloadIfSuccess;
        internal Builder(ErrorCode errorCodeIfFailure, T payLoadIfSuccess) : base(errorCodeIfFailure) {
            _payloadIfSuccess = payLoadIfSuccess;
        }

        public new Builder AssertWithError(Func<bool> condition, ErrorMessage errorMessageToDisplayOnConditionFail) {
            if (!condition()) {
                AddErrorMessage(errorMessageToDisplayOnConditionFail);
            }

            return this;
        }


        public override Result<T> Build() {
            if (ErrorMessages is null) {
                return _payloadIfSuccess;
            }

            return new Error(ErrorCodeIfFailure, ErrorMessages);
        }
    }
}