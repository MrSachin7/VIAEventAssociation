using VIAEventAssociation.Core.QueryContracts.Contracts;

namespace VIAEventAssociation.Core.QueryContracts.Dispatcher;

public class QueryDispatcher : IQueryDispatcher {

    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public async Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query) {
        Type queryType =query.GetType();
        Type answerType = typeof(TAnswer);
        Type handlerType = typeof(IQueryHandler<,>);
        Type genericType = handlerType.MakeGenericType(queryType, answerType);

        dynamic handler = _serviceProvider.GetService(genericType) 
            ?? throw new InvalidOperationException($"Handler for query {query.GetType().Name} not found. Please make sure the handler is registered in the DI container.");
        return await handler.HandleAsync((dynamic)query); 
    }
}