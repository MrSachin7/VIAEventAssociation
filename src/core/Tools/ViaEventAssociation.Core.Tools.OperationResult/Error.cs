﻿
namespace ViaEventAssociation.Core.Tools.OperationResult;

public class Error {

    public ErrorCode ErrorCode { get; set; }
    public IList<ErrorMessage> Messages { get; init; }

    public static Error BadRequest(ErrorMessage message) {
        return new Error(ErrorCode.BadRequest, message);
    } 
    public static Error BadRequest(List<ErrorMessage> messages) {
        return new Error(ErrorCode.BadRequest, messages);
    }

    public static Error Conflict(ErrorMessage message) {
        return new Error(ErrorCode.Conflict, message);
    } 
    public static Error Conflict(List<ErrorMessage> messages) {
        return new Error(ErrorCode.Conflict, messages);
    }

    public static Error NotFound(ErrorMessage message) {
        return new Error(ErrorCode.NotFound, message);
    } 
    public static Error NotFound(List<ErrorMessage> messages) {
        return new Error(ErrorCode.NotFound, messages);
    }

    internal Error(ErrorCode errorCode, IList<ErrorMessage> messages) {
        ErrorCode = errorCode;
        if (messages.Count == 0) {
            throw new Exception("At least one error message is required to create an error");
        }
        Messages = messages;
    }

    private Error(ErrorCode errorCode, ErrorMessage message) {
        ErrorCode = errorCode;
        Messages = new List<ErrorMessage> {message};
    }

    public List<string> GetErrorMessages() {
        return Messages.Select(message => message.ToString()).ToList();
    }

    public override string ToString() {
        List<string> errorMessages= Messages.Select(message => message.ToString()).ToList();
        string joined = string.Join(Environment.NewLine, errorMessages);
        return $"ErrorCode {ErrorCode.Value} : {ErrorCode} \n {joined} ";
    }
}

