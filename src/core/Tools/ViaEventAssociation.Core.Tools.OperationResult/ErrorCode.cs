namespace ViaEventAssociation.Core.Tools.OperationResult;

public class ErrorCode : Enumeration {


    public static readonly ErrorCode Unknown =
        new ErrorCode(0, "Unknown");

    public static readonly ErrorCode BadRequest =
        new ErrorCode(400, "BadRequest");

    public static readonly ErrorCode Unauthorized =
        new ErrorCode(401, "Unauthorized");

    public static readonly ErrorCode Forbidden =
        new ErrorCode(403, "Forbidden");

    public static readonly ErrorCode NotFound =
        new ErrorCode(404, "NotFound");

    public static readonly ErrorCode MethodNotAllowed =
        new ErrorCode(405, "MethodNotAllowed");

    public static readonly ErrorCode Conflict =
        new ErrorCode(409, "Conflict");

    public static readonly ErrorCode PreconditionFailed =
        new ErrorCode(412, "PreconditionFailed");

    public static readonly ErrorCode PayloadTooLarge =
        new ErrorCode(413, "PayloadTooLarge");

    public static readonly ErrorCode UnsupportedMediaType =
        new ErrorCode(415, "UnsupportedMediaType");

    public static readonly ErrorCode UnprocessableEntity =
        new ErrorCode(422, "UnprocessableEntity");

    public static readonly ErrorCode TooManyRequests =
        new ErrorCode(429, "TooManyRequests");

    public static readonly ErrorCode InternalServerError =
        new ErrorCode(500, "InternalServerError");

    public static readonly ErrorCode NotImplemented =
        new ErrorCode(501, "NotImplemented");

    public static readonly ErrorCode BadGateway =
        new ErrorCode(502, "BadGateway");

    public static readonly ErrorCode ServiceUnavailable =
        new ErrorCode(503, "ServiceUnavailable");

    public static readonly ErrorCode GatewayTimeout =
        new ErrorCode(504, "GatewayTimeout");
    




    private ErrorCode(){}

    private ErrorCode(int value, string displayName) : base(value, displayName) {}
}