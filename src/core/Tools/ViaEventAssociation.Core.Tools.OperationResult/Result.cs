using System.Runtime.InteropServices;

namespace ViaEventAssociation.Core.Tools.OperationResult;

public class Result {
    public Error? Error { get; init; }
    public bool IsFailure => Error is not null;
    public bool IsSuccess => !IsFailure;

    protected Result() {
    }

    public Result Combine(Result other) {
        // If both are failures, combine the error messages
        if (IsFailure && other.IsFailure) {
            List<ErrorMessage> combinedErrorMessages = Error!.Messages.Concat(other.Error!.Messages).ToList();
            return new Result() {
                Error = new Error(ErrorCode.BadRequest, combinedErrorMessages)
            };
        }

        // Otherwise, return the failure if there is one

        if (IsFailure) {
            return this;
        }

        if (other.IsFailure) {
            return other;
        }

        // If neither are failures, return success
        return Success();
    }

    public Result<T> WithPayloadIfSuccess<T>(Func<T> payload) {
        if (IsFailure) {
            return Error!;
        }

        return payload();
    }


    public static Result Success() => new Result();

    public static implicit operator Result(Error error)
        => new Result {Error = error};

    public static Result.Builder ToBuilder(ErrorCode errorCodeIfFails) => new(errorCodeIfFails);


    public class Builder {
        protected readonly ErrorCode ErrorCode;
        protected IList<ErrorMessage>? ErrorMessages;

        internal Builder(ErrorCode errorCode) {
            ErrorCode = errorCode;
        }

        public Builder AssertWithError(Func<bool> condition, ErrorMessage errorMessage) {
            if (!condition()) {
                AddErrorMessage(errorMessage);
            }

            return this;
        }

        public Result<T>.Builder WithPayload<T>(T payload) {
            Result<T>.Builder builder = new(ErrorCode, payload) {
                ErrorMessages = ErrorMessages
            };
            return builder;
        }


        private void AddErrorMessage(ErrorMessage errorMessage) {
            if (ErrorMessages is null) {
                ErrorMessages = new List<ErrorMessage>() {errorMessage};
            }
            else {
                ErrorMessages.Add(errorMessage);
            }
        }

        public Result Build() {
            if (ErrorMessages is null) {
                return Result.Success();
            }

            return new Error(ErrorCode, ErrorMessages);
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


    public static implicit operator Result<T>(Error error)
        => new Result<T> {Error = error};

    public static implicit operator Result<T>(T payload)
        => new Result<T> {Payload = payload};

    private void AssertAccessible() {
        if (Error is not null) {
            throw new Exception("Payload is not allowed on an error result");
        }
    }


    public new class Builder(ErrorCode errorCode, T payload) : Result.Builder(errorCode) {
        private readonly T? _payload = payload;

        public new Result<T> Build() {
            if (ErrorMessages is null) {
                return new Result<T> {Payload = _payload};
            }

            return new Result<T> {Error = new Error(ErrorCode, ErrorMessages)};
        }
    }
}