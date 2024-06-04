using VIAEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.QueryContracts.Dispatcher;

public interface IQueryDispatcher {
    Task<Result<TAnswer>> DispatchAsync<TAnswer>(IQuery<TAnswer> query);
}