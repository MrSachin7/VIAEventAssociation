using Microsoft.AspNetCore.Mvc;

namespace VIAEventAssociation.Presentation.WebApi.Endpoints;

public static class QueryEndpointBase {

    public static class WithRequest<TRequest> where TRequest : notnull {
        public abstract class WithResponse<TResponse> : EndpointBase where TResponse : notnull {
            public abstract Task<IResult> HandleAsync(TRequest request);
            
        }
    }

    public static class WithoutRequest {
        public abstract class WithResponse<TResponse> : EndpointBase where TResponse : notnull {
            public abstract Task<IResult> HandleAsync();
            
        }
    }
    
}