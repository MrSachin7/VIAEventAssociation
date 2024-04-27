using VIAEventAssociation.Core.QueryContracts.Contracts;

namespace VIAEventAssociation.Core.QueryContracts.Dispatcher;

public interface IQueryDispatcher {
    Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query);
}