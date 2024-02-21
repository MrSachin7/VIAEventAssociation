using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;

namespace ViaEventAssociation.Core.Tools.OperationResult;

public class Error {

    public ErrorCode ErrorCode { get; set; }
    public string Message { get; init; }
    public List<ValidationError>? ValidationErrors { get; private set; }


    // An example of a factory method
    public static Error BadRequest(string message) => new Error(ErrorCode.BadRequest, message);


    public static Error From(ErrorCode errorCode, string message) => new Error(errorCode, message);

    public bool IsValidationError => ValidationErrors is not null || ValidationErrors?.Count > 0;


    private Error(ErrorCode errorCode, string message) {
        ErrorCode = errorCode;
        Message = message;

    }

    public void AddValidationError(ValidationError validationError) {
        if (ValidationErrors is null) {
            ValidationErrors = new List<ValidationError>();
        }
        ValidationErrors.Add(validationError);
    } 

    public void AddValidationError(string field, string message) {
        AddValidationError(new ValidationError {Field = field, Message = message});
    }

    public void AddValidationErrors(IEnumerable<ValidationError> validationErrors) {
        if (ValidationErrors is null) {
            ValidationErrors = new List<ValidationError>();
        }
        ValidationErrors.AddRange(validationErrors);
    }

}

