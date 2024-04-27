using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.QueryContracts.Contracts;

public interface IQueryHandler<in TQuery, TAnswer> where TQuery : IQuery<TAnswer>{
    public Task<Result<TAnswer>> HandleAsync(TQuery query);
}